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
    using RSBingo_Framework.Exceptions;
    using RSBingo_Framework.Models;
    using RSBingo_Framework.Records;
    using RSBingoBot.Component_interaction_handlers.Select_Component;
    using RSBingoBot.Discord_event_handlers;
    using RSBingoBot.Exceptions;
    using static RSBingo_Framework.DAL.DataFactory;
    using static RSBingo_Common.General;

    // TODO: JR - disable the button if the team's tiles are being changed.
    // Likewise, disable the change tiles button if this button is being interacted with.
    /// <summary>
    /// Handles the interaction with the "Submit evidence" button in a team's board channel.
    /// </summary>
    public class SubmitEvidenceButtonHandler : ComponentInteractionHandler
    {
        private string initialResponseMessagePrefix =
            $"Add evidence by posting a message with a single image, posting another will override the previous." +
            $"{Environment.NewLine}Submitting the evidence will override any previous for the tile.";

        private readonly string tileSelectCustomId = Guid.NewGuid().ToString();

        private string submittedEvidenceURL = string.Empty;
        private SelectComponent tileSelect = null!;
        private DiscordButtonComponent cancelButton = null!;
        private DiscordButtonComponent submitButton = null!;
        private Dictionary<Error, string> errorMessage = new() { };

        /// <inheritdoc/>
        protected override bool ContinueWithNullUser { get { return false; } }

        /// <inheritdoc/>
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            await base.InitialiseAsync(args, info);

            CreateTileSelect();
            submitButton = new DiscordButtonComponent(ButtonStyle.Primary, Guid.NewGuid().ToString(), "Submit");
            cancelButton = new DiscordButtonComponent(ButtonStyle.Primary, Guid.NewGuid().ToString(), "Cancel");

            initialResponseMessagePrefix = $"{args.User.Mention} " + initialResponseMessagePrefix;

            MessagesForCleanup.Add(await args.Interaction.GetOriginalResponseAsync());
            await UpdateOriginalResponse();

            SubscribeComponent(
                new ComponentInteractionDEH.Constraints(user: args.User, customId: tileSelect.CustomId),
                tileSelect.OnInteraction, true);
            SubscribeComponent(
                new ComponentInteractionDEH.Constraints(user: args.User, customId: submitButton.CustomId),
                SubmitButtonInteracted, true);
            SubscribeComponent(
                new ComponentInteractionDEH.Constraints(user: args.User, customId: cancelButton.CustomId),
                CancelButtonInteraction, true);
            SubscribeMessage(
                new MessageCreatedDEH.Constraints(channel: args.Channel, author: args.User, numberOfAttachments: 1),
                EvidencePosted);
        }

        private async Task UpdateOriginalResponse()
        {
            var builder = new DiscordWebhookBuilder()
                .WithContent($"{initialResponseMessagePrefix}{Environment.NewLine}{submittedEvidenceURL}")
                .AddComponents(tileSelect.DiscordComponent)
                .AddComponents(cancelButton, submitButton);

            await OriginalInteractionArgs.Interaction.EditOriginalResponseAsync(builder);
        }

        private void CreateTileSelect()
        {
            IEnumerable<Tile> tiles = Team!.Tiles;

            var options = new List<SelectComponentOption>();
            foreach (Tile tile in tiles)
            {
                options.Add(new SelectComponentItem(tile.Task.Name, tile));
            }

            tileSelect = new(tileSelectCustomId, "Select tiles", maxOptions: MaxTilesOnABoard);
            tileSelect.Options = options;
            tileSelect.Build();
        }

        private async Task EvidencePosted(DiscordClient client, MessageCreateEventArgs args)
        {
            submittedEvidenceURL = args.Message.Attachments[0].Url;

            await UpdateOriginalResponse();

            try
            {
                await args.Message.DeleteAsync();
            }
            catch
            {
                // TODO: JR - create a ButtonComponent class that wraps the DiscordButtonComponent class and handles
                // limiting interactions and disability based on conditions.

                // If there's multiple submission requests by the same person
                // the message may have already been deleted.
            }
        }

        private async Task SubmitButtonInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            (Error error, string errorMessage) = GetSubmissionErrorAndMessage();
            await HandleSubmissionError(error, errorMessage);

            string submittedTiles = SubmitEvidence(
                args.User,
                tileSelect.SelectedItems.Select(i => (Tile)i.value!),
                submittedEvidenceURL);

            await InteractionConcluded();

            await args.Interaction.CreateFollowupMessageAsync(
                new DiscordFollowupMessageBuilder()
                .WithContent($"Evidence has been submitted successfully for the following tiles:{Environment.NewLine}" + submittedTiles)
                .AsEphemeral());
        }

        private (Error, string) GetSubmissionErrorAndMessage()
        {
            Error error = Error.None;
            string errorMessage = string.Empty;

            if (!tileSelect.SelectedItems.Any())
            {
                error = Error.NoTilesSelected;
                errorMessage = "At least one tile must be selected to submit evidence for.";
            }
            else if (submittedEvidenceURL == string.Empty)
            {
                error = Error.NoEvidenceSubmitted;
                errorMessage = "You cannot submit no evidence; please post a message with a single image first.";
            }
            else
            {
                string tilesNotFound = "";

                foreach (SelectComponentItem item in tileSelect.SelectedItems)
                {
                    Tile selectTile = (Tile)item.value!;
                    Tile? retrievedTile = DataWorker.Tiles.GetById(selectTile.RowId);

                    if (retrievedTile == null)
                    {
                        tilesNotFound += selectTile.Task.Name + Environment.NewLine;
                    }
                }

                if (tilesNotFound != "")
                {
                    error = Error.ATileCannotBeFoundInTheDatabase;
                    errorMessage = $"The following tiles could not be found:{Environment.NewLine}{tilesNotFound}They have likely been deleted";
                }
            }

            return (error, errorMessage);
        }

        private async Task HandleSubmissionError(Error error, string errorMessage)
        {
            if (error != Error.None)
            {
                if (error == Error.ATileCannotBeFoundInTheDatabase)
                {
                    CreateTileSelect();
                    await UpdateOriginalResponse();
                }

                throw new ComponentInteractionHandlerException(errorMessage, CurrentInteractionArgs, false,
                    ComponentInteractionHandlerException.ErrorResponseType.CreateFollowUpResponse, true);
            }
        }

        private string SubmitEvidence(DiscordUser discordUser, IEnumerable<Tile> tiles, string url)
        {
            // TODO: JR - also add a flag (somewhere) that tells if the bingo has started.
            // evidence type will still be TileVerification until it has.
            EvidenceRecord.EvidenceType evidenceType = User!.Team.IsBoardVerfied() ?
                EvidenceRecord.EvidenceType.Drop :
                EvidenceRecord.EvidenceType.TileVerification;

            string submittedTiles = "";
            foreach (Tile tile in tiles)
            {
                submittedTiles += tile.Task.Name + Environment.NewLine;
                Evidence? evidence = EvidenceRecord.GetByTile(DataWorker, tile, User, evidenceType);

                if (evidence == null)
                {
                    DataWorker.Evidence.Create(User, tile, url, evidenceType);
                }
                else
                {
                    evidence.Tile = tile;
                    evidence.Url = url;
                    evidence.EvidenceType = (sbyte)evidenceType;
                }
            }

            DataWorker.SaveChanges();

            return submittedTiles;
        }

        private async Task CancelButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            await InteractionConcluded();
        }

        private enum Error
        {
            None,
            NoTilesSelected,
            NoEvidenceSubmitted,
            ATileCannotBeFoundInTheDatabase
        }
    }
}