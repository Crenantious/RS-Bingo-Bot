// <copyright file="AdminEvidenceReaction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingo_Common;
using RSBingoBot.Discord_event_handlers;
using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using static RSBingo_Framework.Records.EvidenceRecord;
using static RSBingo_Framework.DAL.DataFactory;
using static BingoBotCommon;

internal class AdminEvidenceReaction
{
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
            new MessageReactionAddedDEH.Constraints(VerfiedEvidenceChannel, emojiName: EvidenceRejectedEmoji.Name),
            EvidenceRejected);
    }

    private static async Task EvidenceVerified(DiscordClient client, MessageReactionAddEventArgs args) =>
        await HandleMessageReaction(args, VerfiedEvidenceChannel, EvidenceVerifiedEmoji, EvidenceStatus.Verified);

    private static async Task EvidenceRejected(DiscordClient client, MessageReactionAddEventArgs args) =>
        await HandleMessageReaction(args, RejectedEvidenceChannel, EvidenceRejectedEmoji, EvidenceStatus.Rejected);

    private static async Task HandleMessageReaction(MessageReactionAddEventArgs args, DiscordChannel channel,
        DiscordEmoji emoji, EvidenceStatus evidenceStatus)
    {
        if (await UserHasAdminPermission(args) is false) { return; }

        IEnumerable<Evidence> evidence = GetByMessageId(dataWorker, args.Message.Id);
        // This means there is no evidence associated with the message that was reacted to.
        if (evidence.Any() is false) { return; }

        ulong newMessageId = await MoveMessageAndAddReaction(args, channel, emoji);
        UpdateEvidenceDB(args, evidence, evidenceStatus, newMessageId);
    }

    private static async Task<bool> UserHasAdminPermission(MessageReactionAddEventArgs args)
    {
        DiscordMember member = await args.Guild.GetMemberAsync(args.User.Id);
        return member.Permissions.HasPermission(Permissions.Administrator);
    }

    private static async Task<ulong> MoveMessageAndAddReaction(MessageReactionAddEventArgs args,
        DiscordChannel channel, DiscordEmoji emoji)
    {
        // The message must be retrieved this way since args.Message.Content seems to always be null.
        DiscordMessage oldMessage = await args.Message.Channel.GetMessageAsync(args.Message.Id);
        DiscordMessage newMessage = await channel.SendMessageAsync(oldMessage.Content);
        await newMessage.CreateReactionAsync(emoji);
        await oldMessage.DeleteAsync();
        return newMessage.Id;
    }

    private static bool UpdateEvidenceDB(MessageReactionAddEventArgs args, IEnumerable<Evidence> evidence,
        EvidenceStatus evidenceStatus, ulong newMessageId)
    {
        foreach (Evidence e in evidence)
        {
            e.Status = (sbyte)evidenceStatus;
            e.DiscordMessageId = newMessageId;
        }
        dataWorker.SaveChanges();

        return true;
    }
}