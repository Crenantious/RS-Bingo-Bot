// <copyright file="EvidenceReaction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using RSBingoBot.Imaging;
using RSBingoBot.Leaderboard;
using RSBingoBot.Discord_event_handlers;
using RSBingo_Common;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Scoring;
using RSBingo_Framework.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using static BingoBotCommon;
using static RSBingo_Framework.Records.EvidenceRecord;
using static RSBingo_Framework.DAL.DataFactory;

internal class EvidenceReaction
{
    private const string EvidenceSubmittedNotification = "The evidence submitted by {0} for the tile {1} has been {2}.{3}";

    private static IDataWorker dataWorker = CreateDataWorker();

    public static void SetUp()
    {
        MessageReactionAddedDEH messageReactionAddedDEH = (MessageReactionAddedDEH)General.DI.GetService(typeof(MessageReactionAddedDEH));

        messageReactionAddedDEH.Subscribe(
            new MessageReactionAddedDEH.Constraints(PendingReviewEvidenceChannel, emojiName: EvidenceVerifiedEmoji.Name),
            EvidenceVerified);
        messageReactionAddedDEH.Subscribe(
            new MessageReactionAddedDEH.Constraints(PendingReviewEvidenceChannel, emojiName: EvidenceRejectedEmoji.Name),
            EvidenceRejected);

        // These are needed in case the evidence was incorrectly reacted to and the status needs to be changed.
        messageReactionAddedDEH.Subscribe(
            new MessageReactionAddedDEH.Constraints(RejectedEvidenceChannel, emojiName: EvidenceVerifiedEmoji.Name),
            EvidenceVerified);
        messageReactionAddedDEH.Subscribe(
            new MessageReactionAddedDEH.Constraints(VerifiedEvidenceChannel, emojiName: EvidenceRejectedEmoji.Name),
            EvidenceRejected);
    }

    private static async Task EvidenceVerified(DiscordClient client, MessageReactionAddEventArgs args) =>
        await HandleMessageReaction(args, VerifiedEvidenceChannel, EvidenceVerifiedEmoji, EvidenceStatus.Accepted);

    private static async Task EvidenceRejected(DiscordClient client, MessageReactionAddEventArgs args) =>
        await HandleMessageReaction(args, RejectedEvidenceChannel, EvidenceRejectedEmoji, EvidenceStatus.Rejected);

    private static async Task HandleMessageReaction(MessageReactionAddEventArgs args, DiscordChannel channel,
        DiscordEmoji emoji, EvidenceStatus evidenceStatus)
    {
        if (await UserHasAdminPermission(args) is false) { return; }

        Evidence? evidence = GetByMessageId(dataWorker, args.Message.Id);

        // This means there is no evidence associated with the message that was reacted to.
        if (evidence is null) { return; }

        DiscordMessage newMessage = await MoveMessage(args, channel);
        await AddReaction(newMessage, emoji);

        UpdateDB(args, evidence, evidenceStatus, newMessage.Id);
        TeamScore.Update(evidence.Tile);
        dataWorker.SaveChanges();

        await UpdateTeamBoard(evidence);
        await LeaderboardDiscord.Update(dataWorker);
        await PostInTeamEvidenceChannel(args, evidence);
    }

    private static async Task UpdateTeamBoard(Evidence evidence) =>
        await RSBingoBot.DiscordTeam.UpdateBoard(evidence.Tile.Team, BoardImage.UpdateTile(evidence.Tile));

    private static async Task<bool> UserHasAdminPermission(MessageReactionAddEventArgs args)
    {
        DiscordMember member = await args.Guild.GetMemberAsync(args.User.Id);
        return member.Permissions.HasPermission(Permissions.Administrator);
    }

    private static async Task<DiscordMessage> MoveMessage(MessageReactionAddEventArgs args,
        DiscordChannel channel)
    {
        // The message must be retrieved this way since args.Message.Content seems to always be null.
        DiscordMessage oldMessage = await args.Message.Channel.GetMessageAsync(args.Message.Id);
        DiscordMessage newMessage = await channel.SendMessageAsync(oldMessage.Content);
        await oldMessage.DeleteAsync();
        return newMessage;
    }

    private static async Task PostInTeamEvidenceChannel(MessageReactionAddEventArgs args,
        Evidence evidence)
    {
        DiscordChannel evidenceChannel = Guild.GetChannel(evidence.Tile.Team.EvidencelChannelId);
        string status = EvidenceStatusLookup.Get(evidence.Status).ToString().ToLower();

        await evidenceChannel.SendMessageAsync(new DiscordMessageBuilder()
        {
            Content = EvidenceSubmittedNotification.FormatConst(args.User.Username, evidence.Tile.Task.Name,
                status, Environment.NewLine + evidence.Url)
        });
    }

    private static async Task AddReaction(DiscordMessage message, DiscordEmoji emoji) =>
        await message.CreateReactionAsync(emoji);

    private static void UpdateDB(MessageReactionAddEventArgs args, Evidence evidence,
        EvidenceStatus evidenceStatus, ulong newMessageId)
    {
        evidence.Status = (sbyte)evidenceStatus;
        evidence.DiscordMessageId = newMessageId;
        evidence.Tile.SetCompleteStatus(evidence.IsVerified() ? TileRecord.CompleteStatus.Yes : TileRecord.CompleteStatus.No);
    }
}