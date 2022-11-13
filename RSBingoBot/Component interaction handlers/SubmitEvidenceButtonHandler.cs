// <copyright file="SubmitEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using RSBingo_Framework.DAL;
    using RSBingo_Framework.Exceptions;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using RSBingo_Framework.Records;
    using RSBingoBot.Discord_event_handlers;
    using static RSBingo_Framework.DAL.DataFactory;

    /// <summary>
    /// Handles the interaction with the "Submit evidence" button in a team's board channel.
    /// </summary>
    public class SubmitEvidenceButtonHandler : ComponentInteractionHandler
    {
        private readonly List<string> tileItems = new() { "Bandos Chestplate", "Bandos Tassets", "Dragon Rider Lance" };
        private readonly string tileSelectCustomId = Guid.NewGuid().ToString();

        private string initialResponseMessagePrefix =
            $"Add evidence by posting a message with a single image, posting another will override the previous." +
            $"\nSubmitting the evidence will override any previous for the tile.";
        private string submittedEvidenceURL = string.Empty;
        private DiscordSelectComponent tileSelect = null!;
        private DiscordButtonComponent cancelButton = null!;
        private DiscordButtonComponent submitButton = null!;
        private ICollection<Tile> selectedTiles = null!;

        /// <inheritdoc/>
        protected override bool ContinueWithNullUser { get { return false; } }

        /// <inheritdoc/>
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            await base.InitialiseAsync(args, info);

            submitButton = new DiscordButtonComponent(ButtonStyle.Primary, Guid.NewGuid().ToString(), "Submit");
            cancelButton = new DiscordButtonComponent(ButtonStyle.Primary, Guid.NewGuid().ToString(), "Cancel");

            initialResponseMessagePrefix = $"{args.User.Mention} " + initialResponseMessagePrefix;

            // Try get existing evidence from DB.

            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            MessagesForCleanup.Add(await args.Interaction.GetOriginalResponseAsync());
            await UpdateOriginalResponse(args.User.Id);

            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: tileSelect.CustomId), TileSelectInteraction);
            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: submitButton.CustomId), SubmitButtonInteraction);
            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: cancelButton.CustomId), CancelButtonInteraction);
            SubscribeMessage(new MessageCreatedDEH.Constraints(channel: args.Channel, author: args.User, numberOfAttachments: 1), EvidencePosted);
        }

        private void CreateTileSelect(ulong discordUserId)
        {
            ICollection<Tile> tiles = User.Team.Tiles;

            var tileSelectOptions = new List<DiscordSelectComponentOption>();
            foreach (Tile tile in tiles)
            {
                tileSelectOptions.Add(new DiscordSelectComponentOption(tile.Task.Name, tile.RowId.ToString(), isDefault: selectedTiles.Contains(tile)));
            }

            // For maxOptions, the number cannot exceed the amount of options or there'll be an error
            tileSelect = new DiscordSelectComponent(tileSelectCustomId, "Select tiles", tileSelectOptions, false, 1, tileItems.Count);
        }

        private async Task<object> TileSelectInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            HashSet<int> tileIds = new();

            foreach (string tileIdStr in args.Values)
            {
                if (int.TryParse(tileIdStr, out int tileId))
                {
                    tileIds.Add(tileId);
                }
            }

            if (tileIds.Count != args.Values.Length) { return Task.FromException(new InvalidTileIdException()); }

            selectedTiles = DataWorker.Tiles.GetByIds(tileIds).ToList();
            if (selectedTiles.Count != tileIds.Count) { return Task.FromException(new CouldNotFindTileIdException()); }

            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            return Task.CompletedTask;
        }

        private async Task EvidencePosted(DiscordClient client, MessageCreateEventArgs args)
        {
            submittedEvidenceURL = args.Message.Attachments[0].Url;
            await UpdateOriginalResponse(args.Author.Id);

            try
            {
                await args.Message.DeleteAsync();
            }
            catch
            {
                // If there's multiple submission requests by the same person
                // the message may have already been deleted.
            }
        }

        private async Task SubmitButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            string content;

            if (selectedTiles.Count == 0)
            {
                content = "At least one tile must be selected to submit evidence for.";
            }
            else if (submittedEvidenceURL == string.Empty)
            {
                content = "You cannot submit no evidence; please post a message with a single image to do so.";
            }
            else
            {
                SubmitEvidence(args.User, selectedTiles, submittedEvidenceURL);
                content = "Evidence has been submitted successfully for the following tiles:\n" +
                              string.Join("\n", selectedTiles);
                await InteractionConcluded();
            }

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                .WithContent(content)
                .AsEphemeral());
        }

        private async Task CancelButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            await InteractionConcluded();
        }

        private async Task UpdateOriginalResponse(ulong discordUserId)
        {
            CreateTileSelect(discordUserId);

            var builder = new DiscordWebhookBuilder()
                .WithContent($"{initialResponseMessagePrefix}\n{submittedEvidenceURL}")
                .AddComponents(tileSelect)
                .AddComponents(cancelButton, submitButton);

            await OriginalInteractionArgs.Interaction.EditOriginalResponseAsync(builder);
        }

        private void SubmitEvidence(DiscordUser discordUser, ICollection<Tile> tiles, string url)
        {
            if (User == null) { return; }

            EvidenceRecord.EvidenceType evidenceType = User.Team.IsBoardVerfied() ?
                EvidenceRecord.EvidenceType.Drop :
                EvidenceRecord.EvidenceType.TileVerification;

            foreach (Tile tile in tiles)
            {
                DataWorker.Evidence.Create(User, tile, url, evidenceType);
            }

            DataWorker.SaveChanges();

            var builder = new DiscordMessageBuilder()
                .WithContent($"{discordUser.Mention} has submitted evidence for the following tiles: ....{url}");
            // Post to a centralised evidence channel.
            //Info.Team.SubmittedEvidenceChannel.SendMessageAsync(builder);
        }
    }
}