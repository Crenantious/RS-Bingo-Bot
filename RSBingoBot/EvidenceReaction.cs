// TODO: JR - refactor int o requests

//// <copyright file="EvidenceReaction.cs" company="PlaceholderCompany">
//// Copyright (c) PlaceholderCompany. All rights reserved.
//// </copyright>

//namespace RSBingoBot;

//using DiscordLibrary.DiscordEventHandlers;
//using DSharpPlus;
//using DSharpPlus.Entities;
//using DSharpPlus.EventArgs;
//using RSBingo_Common;
//using RSBingo_Framework.Interfaces;
//using RSBingo_Framework.Models;
//using RSBingo_Framework.Records;
//using RSBingo_Framework.Scoring;
//using RSBingoBot.Imaging;
//using RSBingoBot.Leaderboard;
//using static BingoBotCommon;
//using static RSBingo_Framework.DAL.DataFactory;
//using static RSBingo_Framework.Records.EvidenceRecord;

//internal class EvidenceReaction
//{
//    private const string EvidenceCheckedTitle = "Evidence";
//    private const string EvidenceCheckedMessage = "[Evidence]({0}) submitted by {1} for {2} has been {3}.";
//    private static IDataWorker dataWorker = CreateDataWorker();

//    public static void SetUp()
//    {
//        MessageReactionAddedDEH messageReactionAddedDEH = (MessageReactionAddedDEH)General.DI.GetService(typeof(MessageReactionAddedDEH));

//        messageReactionAddedDEH.Subscribe(
//            new MessageReactionAddedDEH.Constraints(PendingReviewEvidenceChannel, emojiName: EvidenceVerifiedEmoji.Name),
//            (c, e) => EvidenceVerified(c, e, true));
//        messageReactionAddedDEH.Subscribe(
//            new MessageReactionAddedDEH.Constraints(PendingReviewEvidenceChannel, emojiName: EvidenceRejectedEmoji.Name),
//            (c, e) => EvidenceRejected(c, e, true));

//        // These are needed in case the evidence was incorrectly reacted to and the status needs to be changed.
//        messageReactionAddedDEH.Subscribe(
//            new MessageReactionAddedDEH.Constraints(RejectedEvidenceChannel, emojiName: EvidenceVerifiedEmoji.Name),
//            (c, e) => EvidenceVerified(c, e, false));
//        messageReactionAddedDEH.Subscribe(
//            new MessageReactionAddedDEH.Constraints(VerifiedEvidenceChannel, emojiName: EvidenceRejectedEmoji.Name),
//            (c, e) => EvidenceRejected(c, e, false));
//    }

//    private static async Task EvidenceVerified(DiscordClient client, MessageReactionAddEventArgs args, bool wasPending) =>
//        await HandleMessageReaction(args, VerifiedEvidenceChannel, EvidenceVerifiedEmoji, EvidenceStatus.Accepted, wasPending);

//    private static async Task EvidenceRejected(DiscordClient client, MessageReactionAddEventArgs args, bool wasPending) =>
//        await HandleMessageReaction(args, RejectedEvidenceChannel, EvidenceRejectedEmoji, EvidenceStatus.Rejected, wasPending);

//    private static async Task HandleMessageReaction(MessageReactionAddEventArgs args, DiscordChannel channel,
//        DiscordEmoji emoji, EvidenceStatus evidenceStatus, bool wasPending)
//    {
//        if (await DoesNotHasPermission(args)) { return; }

//        Evidence? evidence = GetByMessageId(dataWorker, args.Message.Id);

//        // This means there is no evidence associated with the message that was reacted to.
//        if (evidence is null) { return; }

//        DiscordMessage newMessage = await UpdatEvidenceMessage(args, channel, emoji);
//        UpdateDB(args, evidenceStatus, evidence, newMessage, wasPending);
//        await UpdateDiscord(args, evidence);
//    }

//    // This should be done in a common global method.
//    private static async Task<bool> DoesNotHasPermission(MessageReactionAddEventArgs args) =>
//        (await args.Guild.GetMemberAsync(args.User.Id))
//            .Roles.FirstOrDefault(r => r.Name == "Host") is null;

//    private static async Task<DiscordMessage> UpdatEvidenceMessage(MessageReactionAddEventArgs args, DiscordChannel channel, DiscordEmoji emoji)
//    {
//        DiscordMessage newMessage = await MoveMessage(args, channel);
//        await AddReaction(newMessage, emoji);
//        return newMessage;
//    }

//    private static async Task UpdateDiscord(MessageReactionAddEventArgs args, Evidence? evidence)
//    {
//        await UpdateTeamBoard(evidence);
//        await LeaderboardDiscord.Update(dataWorker);
//        await PostInTeamEvidenceChannel(args, evidence);
//    }

//    private static void UpdateDB(MessageReactionAddEventArgs args, EvidenceStatus evidenceStatus, Evidence? evidence,
//        DiscordMessage newMessage, bool wasPending)
//    {
//        UpdateDBEvidence(args, evidence, evidenceStatus, newMessage.Id);
//        if ((wasPending && evidenceStatus == EvidenceStatus.Rejected) is false) { TeamScore.Update(evidence.Tile); }
//        dataWorker.SaveChanges();
//    }

//    private static async Task UpdateTeamBoard(Evidence evidence) =>
//        await RSBingoBot.DiscordTeamOld.UpdateBoard(evidence.Tile.Team, BoardImage.UpdateTile(evidence.Tile));

//    private static async Task<DiscordMessage> MoveMessage(MessageReactionAddEventArgs args,
//        DiscordChannel channel)
//    {
//        // The message must be retrieved this way since args.Message.Content seems to always be null.
//        DiscordMessage oldMessage = await args.Message.Channel.GetMessageAsync(args.Message.Id);
//        DiscordMessage newMessage = await channel.SendMessageAsync(oldMessage.Content);
//        await oldMessage.DeleteAsync();
//        return newMessage;
//    }

//    private static async Task PostInTeamEvidenceChannel(MessageReactionAddEventArgs args,
//        Evidence evidence)
//    {
//        DiscordChannel evidenceChannel = Guild.GetChannel(evidence.Tile.Team.EvidenceChannelId);
//        string status = EvidenceStatusLookup.Get(evidence.Status).ToString().ToLower();

//        DiscordEmbedBuilder builder = new();
//        builder.AddField(EvidenceCheckedTitle, EvidenceCheckedMessage.FormatConst(evidence.Url, args.User.Username, evidence.Tile.Task.Name,
//                status));

//        await evidenceChannel.SendMessageAsync(builder);
//    }

//    private static async Task AddReaction(DiscordMessage message, DiscordEmoji emoji) =>
//        await message.CreateReactionAsync(emoji);

//    private static void UpdateDBEvidence(MessageReactionAddEventArgs args, Evidence evidence,
//        EvidenceStatus evidenceStatus, ulong newMessageId)
//    {
//        evidence.Status = (sbyte)evidenceStatus;
//        evidence.DiscordMessageId = newMessageId;
//        evidence.Tile.SetCompleteStatus(evidence.IsVerified() ? TileRecord.CompleteStatus.Yes : TileRecord.CompleteStatus.No);
//    }
//}