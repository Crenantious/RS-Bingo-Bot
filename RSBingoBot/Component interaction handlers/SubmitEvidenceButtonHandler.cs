// <copyright file="SubmitEvidenceButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Exceptions;
using RSBingoBot.Discord_event_handlers;
using RSBingoBot.Component_interaction_handlers.Select_Component;
using static RSBingo_Common.General;
using static RSBingo_Framework.DAL.DataFactory;


// TODO: JR - disable the button if the team's tiles are being changed.
// Likewise, disable the change tiles button if this button is being interacted with.
// TODO: JR - split this into two different buttons; one for evidence and one for drops.
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
    private Dictionary<SubmissionError, string> errorMessage = new() { };

    /// <inheritdoc/>
    protected override bool ContinueWithNullUser { get; } = false;
    protected override bool CreateAutoResponse { get; } = true;

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

        InitialDEHSubscriptions(args);
    }

    private void InitialDEHSubscriptions(ComponentInteractionCreateEventArgs args)
    {
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
        tileSelect.SelectOptions = options;
        tileSelect.Build();
    }

    private async Task EvidencePosted(DiscordClient client, MessageCreateEventArgs args)
    {
        submittedEvidenceURL = args.Message.Attachments[0].Url;

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

        string submittedTiles = await SubmitEvidence(
            args.User,
            tileSelect.SelectedItems.Select(i => (Tile)i.value!),
            submittedEvidenceURL);

        await ConcludeInteraction();

        await args.Interaction.CreateFollowupMessageAsync(
            new DiscordFollowupMessageBuilder()
            .WithContent($"Evidence has been submitted successfully for the following tiles:{Environment.NewLine}{submittedTiles}")
            .AsEphemeral());
    }

    private (SubmissionError, string) GetSubmissionErrorAndMessage()
    {
        SubmissionError error = SubmissionError.None;
        string errorMessage = string.Empty;

        if (!tileSelect.SelectedItems.Any())
        {
            error = SubmissionError.NoTilesSelected;
            errorMessage = "At least one tile must be selected to submit evidence for.";
        }
        else if (submittedEvidenceURL == string.Empty)
        {
            error = SubmissionError.NoEvidenceSubmitted;
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
                error = SubmissionError.ATileCannotBeFoundInTheDatabase;
                errorMessage = $"The following tiles could not be found:{Environment.NewLine}{tilesNotFound}They have likely been deleted";
            }
        }

        return (error, errorMessage);
    }

    private async Task HandleSubmissionError(SubmissionError error, string errorMessage)
    {
        if (error != SubmissionError.None)
        {
            if (error == SubmissionError.ATileCannotBeFoundInTheDatabase)
            {
                CreateTileSelect();
                await UpdateOriginalResponse();
            }

            throw new ComponentInteractionHandlerException(errorMessage, CurrentInteractionArgs, false,
                ComponentInteractionHandlerException.ErrorResponseType.CreateFollowUpResponse, true);
        }
    }

    private async Task<string> SubmitEvidence(DiscordUser discordUser, IEnumerable<Tile> tiles, string url)
    {
        // TODO: JR - delete any currently submitted evidence messages for the tiles.
        // TODO: JR - post one evidence message per tile so they can be individually handled (deleted in this case).
        ulong discordMessageId = await SubmitEvidenceDiscord(tiles, url);
        string submittedTiles = SubmitEvidenceDB(tiles, url, discordMessageId);
        return submittedTiles;
    }

    private async Task<ulong> SubmitEvidenceDiscord(IEnumerable<Tile> tiles, string url)
    {
        StringBuilder submittedTiles = new();

        foreach (Tile tile in tiles)
        {
            submittedTiles.AppendLine(tile.Task.Name);
        }

        DiscordMessage message = await PendingReviewEvidenceChannel.SendMessageAsync(new DiscordMessageBuilder()
            .WithContent($"{CurrentInteractionArgs.User.Mention} has submitted evidence for the following tiles:" +
            $"{Environment.NewLine}{submittedTiles}{Environment.NewLine}{url}"));
        return message.Id;
    }

    private string SubmitEvidenceDB(IEnumerable<Tile> tiles, string url, ulong discordMessageId)
    {
        //EvidenceRecord.EvidenceType evidenceType = User!.Team.IsBoardVerfied() && HasCompetitionStarted?
        EvidenceRecord.EvidenceType evidenceType = HasCompetitionStarted ?
            EvidenceRecord.EvidenceType.Drop :
            EvidenceRecord.EvidenceType.TileVerification;

        StringBuilder submittedTiles = new();

        foreach (Tile tile in tiles)
        {
            submittedTiles.AppendLine(tile.Task.Name);
            Evidence? evidence = EvidenceRecord.GetByTileAndUser(DataWorker, tile, User);

            if (evidence == null)
            {
                DataWorker.Evidence.Create(User, tile, url, evidenceType, discordMessageId);
            }
            else
            {
                evidence.Tile = tile;
                evidence.Url = url;
                evidence.EvidenceType = (sbyte)evidenceType;
                evidence.DiscordMessageId = discordMessageId;
                evidence.Status = (sbyte)EvidenceRecord.EvidenceStatus.PendingReview;
            }
        }

        DataWorker.SaveChanges();
        return submittedTiles.ToString();
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