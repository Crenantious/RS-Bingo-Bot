// <copyright file="SubmitImageForTileButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Discord_event_handlers;
using RSBingoBot.Component_interaction_handlers.Select_Component;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingoBot.MessageUtilities;

/// <summary>
/// Handles the interaction with a button that requires submitting an image for a tile in a team's board channel.
/// </summary>
// TODO: - refactor. Split into helper classes most likely.
public abstract class SubmitImageForTileButtonHandler : ComponentInteractionHandler
{
    // This is for when the bug related to re-creating the select component gets fixed.
    //private const string TileIsAlreadyCompleteError = "A selected tile has already been completed so evidence can no longer be submitted for it. " +
    //                    "The list has been updated.";
    private const string TileIsAlreadyCompleteError = "A selected tile has already been completed; evidence can no longer be submitted for it. " +
                        "Please press the button again to get a refreshed list.";
    private string tileSelectCustomId = Guid.NewGuid().ToString();

    private DiscordButtonComponent cancelButton = null!;
    private DiscordButtonComponent submitButton = null!;
    private string initialResponseMessagePrefix = 
        "Add evidence by posting a message with a single image, posting another will override the previous." +
        $"{Environment.NewLine}Submitting the evidence will override any previous you have submitted for the tile.";

    protected abstract int TileSelectMaxOptions { get; }
    protected abstract EvidenceRecord.EvidenceType EvidenceType { get; }

    protected SelectComponent TileSelect { get; private set; } = null!;
    protected string SubmittedImageURL { get; private set; } = string.Empty;

    /// <inheritdoc/>
    protected override bool ContinueWithNullUser { get; } = false;
    protected override bool CreateAutoResponse { get; } = true;

    /// <inheritdoc/>
    public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
    {
        await base.InitialiseAsync(args, info);

        MessagesForCleanup.Add(await args.Interaction.GetOriginalResponseAsync());

        if (Team!.Tiles.Any() is false)
        {
            await Followup(args.Interaction, "There are no tiles to submit evidence for.", true);
            await ConcludeInteraction();
            return;
        }

        CreateTileSelect();
        submitButton = new DiscordButtonComponent(ButtonStyle.Primary, Guid.NewGuid().ToString(), "Submit");
        cancelButton = new DiscordButtonComponent(ButtonStyle.Primary, Guid.NewGuid().ToString(), "Cancel");

        initialResponseMessagePrefix = $"{args.User.Mention} " + initialResponseMessagePrefix;

        await UpdateOriginalResponse();

        InitialDEHSubscriptions(args);
    }

    protected abstract IEnumerable<Tile> GetTileSelectTiles();

    private void InitialDEHSubscriptions(ComponentInteractionCreateEventArgs args)
    {
        SubscribeComponent(
            new ComponentInteractionDEH.Constraints(user: args.User, customId: TileSelect.CustomId),
            TileSelect.OnInteraction, true);
        SubscribeComponent(
            new ComponentInteractionDEH.Constraints(user: args.User, customId: submitButton.CustomId),
            SubmitButtonInteracted, true);
        SubscribeComponent(
            new ComponentInteractionDEH.Constraints(user: args.User, customId: cancelButton.CustomId),
            CancelButtonInteraction, true);
        SubscribeMessage(
            new MessageCreatedDEH.Constraints(channel: args.Channel, author: args.User, numberOfAttachments: 1),
            ImagePosted);
    }

    private async Task UpdateOriginalResponse()
    {
        var builder = new DiscordWebhookBuilder()
            .WithContent($"{initialResponseMessagePrefix}{Environment.NewLine}{SubmittedImageURL}")
            .AddComponents(TileSelect.DiscordComponent)
            .AddComponents(cancelButton, submitButton);

        await OriginalInteractionArgs.Interaction.EditOriginalResponseAsync(builder);
    }

    private void CreateTileSelect()
    {
        IEnumerable<Tile> tiles = GetTileSelectTiles();
        //tileSelectCustomId = new Guid().ToString();
        var options = new List<SelectComponentOption>();
        foreach (Tile tile in tiles)
        {
            options.Add(new SelectComponentItem(tile.Task.Name, tile));
        }

        TileSelect = new(tileSelectCustomId, "Select tiles", SelectOptionSelected, SelectOptionSelected, maxOptions: TileSelectMaxOptions);
        TileSelect.SelectOptions = options;
        TileSelect.Build();
    }

    private async Task SelectOptionSelected(InteractionCreateEventArgs args) =>
        await UpdateOriginalResponse();

    private async Task ImagePosted(DiscordClient client, MessageCreateEventArgs args)
    {
        SubmittedImageURL = args.Message.Attachments[0].Url;
        await UpdateOriginalResponse();

        try { await args.Message.DeleteAsync(); }
        catch
        {
            // If there's multiple submission requests by the same person
            // the message may have already been deleted.

            // TODO: JR - create a ButtonComponent class that wraps the DiscordButtonComponent class and handles
            // limiting interactions and disability based on conditions.
        }
    }

    private async Task SubmitButtonInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        (SubmissionError error, string errorMessage) = GetSubmissionErrorAndMessage();
        await HandleSubmissionError(error, errorMessage);

        IEnumerable<Tile> tiles = TileSelect.SelectedItems.Select(i => (Tile)i.value!);
        if (await ValidateSelection(args, tiles) is false)
        {
            // HACK: - not sure why a new dw is required to refresh the team's tiles.
            // TODO: JR - when re-creating the tile select, the selection is broken. It gets created correctly with all the tiles
            // split into pages, but when selecting an option, it seems as though it's options are that of page 1.
            // And so it tries to get the page as an index of those options. The placeholder stays as the default, not sure
            // if an option is actually counted as being selected.
            //DataWorker = CreateDataWorker();
            //CreateTileSelect();
            //await UpdateOriginalResponse();
            //await Followup(args.Interaction, TileIsAlreadyCompleteError, true);

            // TODO: replace this with the above code and fix it. This is just temporary so the competition can run on  time,
            await ConcludeInteraction();
            await Followup(args.Interaction, TileIsAlreadyCompleteError, true);
            return;
        }

        await DeleteCurrentEvidenceMessages(tiles);
        string submittedTiles = await SubmitEvidence(tiles);

        await ConcludeInteraction();

        await args.Interaction.CreateFollowupMessageAsync(
            new DiscordFollowupMessageBuilder()
            .WithContent($"Evidence has been submitted successfully for the following tiles:{Environment.NewLine}{submittedTiles}")
            .AsEphemeral());
    }

    private async Task<bool> ValidateSelection(ComponentInteractionCreateEventArgs args, IEnumerable<Tile> tiles)
    {
        // TODO: find a way to not require creating a new dw. Currently, the tile will not show as complete if the class' dw is used.
        IDataWorker dw = CreateDataWorker();
        List<Tile> invalidTiles = new(tiles.Count());

        foreach (Tile tile in tiles)
        {
            Tile refreshedTile = dw.Tiles.GetById(tile.RowId)!;
            if (refreshedTile.IsCompleteAsBool())
            {
                invalidTiles.Add(refreshedTile);
            }
        }

        return invalidTiles.Any() is false;
    }

    private async Task DeleteCurrentEvidenceMessages(IEnumerable<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            Evidence? evidence = EvidenceRecord.GetByTileUserAndType(DataWorker, tile, User!, EvidenceType);
            if (evidence is null) { continue; }

            await DeletePendingEvidenceMessage(evidence.DiscordMessageId);
        }
    }

    private async Task DeletePendingEvidenceMessage(ulong messageId)
    {
        try
        {
            DiscordMessage message = await PendingReviewEvidenceChannel.GetMessageAsync(messageId);
            await message.DeleteAsync();
        }
        catch { return; }
    }

    private async Task<string> SubmitEvidence(IEnumerable<Tile> tiles)
    {
        StringBuilder submittedTiles = new();

        foreach (Tile tile in tiles)
        {
            submittedTiles.AppendLine(tile.Task.Name);

            ulong discordMessageId = await SubmitEvidenceToDiscord(tile);
            SubmitEvidenceToDB(tile, discordMessageId);
        }
        DataWorker.SaveChanges();
        return submittedTiles.ToString();
    }

    private async Task<ulong> SubmitEvidenceToDiscord(Tile tile)
    {
        DiscordMessage message = await PendingReviewEvidenceChannel.SendMessageAsync(new DiscordMessageBuilder()
            .WithContent($"{CurrentInteractionArgs.User.Mention} has submitted {EvidenceType} evidence for {tile.Task.Name}" +
            $"{Environment.NewLine}{SubmittedImageURL}"));
        return message.Id;
    }

    private void SubmitEvidenceToDB(Tile tile, ulong discordMessageId)
    {
        Evidence? evidence = EvidenceRecord.GetByTileUserAndType(DataWorker, tile, User!, EvidenceType);

        if (evidence == null)
        {
            DataWorker.Evidence.Create(User!, tile, SubmittedImageURL, EvidenceType, discordMessageId);
            return;
        }

        evidence.Url = SubmittedImageURL;
        evidence.Status = (sbyte)EvidenceRecord.EvidenceStatus.PendingReview;
        evidence.DiscordMessageId = discordMessageId;
    }

    private (SubmissionError, string) GetSubmissionErrorAndMessage()
    {
        SubmissionError error = SubmissionError.None;
        string errorMessage = string.Empty;

        if (!TileSelect.SelectedItems.Any())
        {
            error = SubmissionError.NoTilesSelected;
            errorMessage = "At least one tile must be selected to submit evidence for.";
        }
        else if (SubmittedImageURL == string.Empty)
        {
            error = SubmissionError.NoEvidenceSubmitted;
            errorMessage = "You cannot submit no evidence; please post a message with a single image first.";
        }
        else
        {
            string tilesNotFound = "";

            foreach (SelectComponentItem item in TileSelect.SelectedItems)
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
                error = SubmissionError.ATileCannotBeFoundInTheDatabase;
                errorMessage = $"The following tiles could not be found:{Environment.NewLine}{tilesNotFound}They have likely been deleted";
            }
        }

        return (error, errorMessage);
    }

    private async Task HandleSubmissionError(SubmissionError error, string errorMessage)
    {
        if (error == SubmissionError.None) { return; }

        if (error == SubmissionError.ATileCannotBeFoundInTheDatabase)
        {
            CreateTileSelect();
            await UpdateOriginalResponse();
        }

        throw new ComponentInteractionHandlerException(errorMessage, CurrentInteractionArgs, false,
            ComponentInteractionHandlerException.ErrorResponseType.CreateFollowUpResponse, true);
    }

    private async Task CancelButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args) =>
        await ConcludeInteraction();

    private enum SubmissionError
    {
        None,
        NoTilesSelected,
        NoEvidenceSubmitted,
        ATileCannotBeFoundInTheDatabase
    }
}