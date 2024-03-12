// <copyright file="SubmitEvidenceSubmitButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentValidation;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.Requests.Validation;

internal class SubmitEvidenceSubmitButtonValidator : BingoValidator<SubmitEvidenceSubmitButtonRequest>
{
    private const string NoTilesSelectedError = "At least one tile must be selected to submit evidence for.";
    private const string NoEvidenceSubmittedError = "You cannot submit no evidence; please post a message with a single image first.";
    private const string EvidenceAlreadyVerifiedError = "A tile you have selected has already has verified evidence from you, " +
        "please re-open this interaction to get a refreshed list.";
    private const string TileAlreadyCompletedError = "A tile you have selected has already been completed, " +
        "please re-open this interaction to get a refreshed list.";

    public SubmitEvidenceSubmitButtonValidator()
    {
        When(r => r.EvidenceType == EvidenceRecord.EvidenceType.TileVerification, TileVerificationValidation);
        When(r => r.EvidenceType == EvidenceRecord.EvidenceType.Drop, DropValidation);
    }

    private void TileVerificationValidation()
    {
        CompetitionNotStarted();

        When(r => General.HasCompetitionStarted is false, () =>
        {
            RuleFor(r => r.DTO.Tiles.Any())
                .Equal(true)
                .WithMessage(NoTilesSelectedError);
        });

        When(r => General.HasCompetitionStarted is false, () =>
        {
            RuleFor(r => ValidateTiles(r.DTO.Tiles, r))
                .Equal(false)
                .WithMessage(EvidenceAlreadyVerifiedError);
        });

        When(r => General.HasCompetitionStarted is false, () =>
        {
            NotNull(r => r.DTO.EvidenceUrl, NoEvidenceSubmittedError);
        });
    }

    private void DropValidation()
    {
        CompetitionStarted();

        When(r => General.HasCompetitionStarted, () =>
        {
            RuleFor(r => r.DTO.Tiles.Any())
                .Equal(true)
                .WithMessage(NoTilesSelectedError);
        });

        When(r => General.HasCompetitionStarted, () =>
        {
            RuleFor(r => ValidateTiles(r.DTO.Tiles, r))
                .Equal(false)
                .WithMessage(TileAlreadyCompletedError);
        });

        When(r => General.HasCompetitionStarted, () =>
        {
            NotNull(r => r.DTO.EvidenceUrl, NoEvidenceSubmittedError);
        });
    }

    private bool ValidateTiles(IEnumerable<Tile> tiles, SubmitEvidenceSubmitButtonRequest request)
    {
        // We must get a refreshed version of the tiles to ensure we have an up-to-date version of the evidence.
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        var refreshedTiles = dataWorker.Tiles.GetByIds(tiles.Select(t => t.RowId));
        return SubmitEvidenceTileValidator.Validate(refreshedTiles, request.EvidenceType, request.User.DiscordUserId);
    }

    protected override IEnumerable<SemaphoreSlim> GetSemaphores(SubmitEvidenceSubmitButtonRequest request, RequestSemaphores semaphores) =>
        new List<SemaphoreSlim>()
        {
            semaphores.GetTeamDatabase(request.DiscordTeam.Id),
            semaphores.GetEvidence(request.DiscordTeam.Id)
        };
}