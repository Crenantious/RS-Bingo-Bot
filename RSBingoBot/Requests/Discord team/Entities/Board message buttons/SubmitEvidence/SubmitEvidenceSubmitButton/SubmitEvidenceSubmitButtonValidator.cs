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
    private const string TileAlreadyVerifiedError = "A tile you have selected has already been verified, please re-open this interaction " +
        "to get a refreshed list.";

    public SubmitEvidenceSubmitButtonValidator()
    {
        When(r => r.EvidenceType == EvidenceRecord.EvidenceType.TileVerification, CompetitionNotStarted);

        When(r => General.HasCompetitionStarted is false, () =>
        {
            RuleFor(r => r.DTO.Tiles.Any())
            .Equal(true)
            .WithMessage(NoTilesSelectedError);
        });

        When(r => General.HasCompetitionStarted is false, () =>
        {
            RuleFor(r => HasAVerifiedTile(r.DTO.Tiles, r))
            .Equal(false)
            .WithMessage(TileAlreadyVerifiedError);
        });

        When(r => General.HasCompetitionStarted is false, () =>
        {
            NotNull(r => r.DTO.EvidenceUrl, NoEvidenceSubmittedError);
        });
    }

    private bool HasAVerifiedTile(IEnumerable<Tile> tiles, SubmitEvidenceSubmitButtonRequest request)
    {
        // We must get a refreshed version of the tiles to ensure we have an up-to-date version of the evidence.
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        var refreshedTiles = dataWorker.Tiles.GetByIds(tiles.Select(t => t.RowId));
        return refreshedTiles.Any(t => HasVerifiedEvidence(dataWorker, t, request));
    }

    private static bool HasVerifiedEvidence(IDataWorker dataWorker, Tile t, SubmitEvidenceSubmitButtonRequest request) =>
        t.Evidence.Any(e => e.IsVerified() &&
            e.User.DiscordUserId == request.User.DiscordUserId &&
            GetEvidenceType(e) == request.EvidenceType);

    private static EvidenceRecord.EvidenceType GetEvidenceType(Evidence e) =>
        EvidenceRecord.EvidenceTypeLookup.Get(e.EvidenceType);

    protected override IEnumerable<SemaphoreSlim> GetSemaphores(SubmitEvidenceSubmitButtonRequest request, RequestSemaphores semaphores) =>
        new List<SemaphoreSlim>()
        {
            semaphores.GetTeamDatabase(request.DiscordTeam.Id),
            semaphores.GetEvidence(request.DiscordTeam.Id)
        };
}