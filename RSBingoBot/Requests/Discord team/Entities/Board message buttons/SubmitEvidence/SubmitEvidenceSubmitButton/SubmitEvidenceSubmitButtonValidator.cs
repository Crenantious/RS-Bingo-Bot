// <copyright file="SubmitEvidenceSubmitButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using FluentValidation;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.TileValidators;
using RSBingoBot.Requests.Validation;

internal class SubmitEvidenceSubmitButtonValidator : BingoValidator<SubmitEvidenceSubmitButtonRequest>
{
    private const string NoTilesSelectedError = "At least one tile must be selected to submit evidence for.";
    private const string NoEvidenceSubmittedError = "You cannot submit no evidence; please post a message with a single image first.";

    private readonly ISubmitEvidenceTSV tileValidator;

    private string TSVError = string.Empty;

    public SubmitEvidenceSubmitButtonValidator(ISubmitEvidenceTSV tileValidator)
    {
        UserOnTeam(r => (r.GetDiscordInteraction().User, r.DiscordTeam.Id), true);

        When(r => r.EvidenceType == EvidenceRecord.EvidenceType.TileVerification, TileVerificationValidation);
        When(r => r.EvidenceType == EvidenceRecord.EvidenceType.Drop, DropValidation);

        this.tileValidator = tileValidator;

        NotNull(r => r.DTO.EvidenceUrl, NoEvidenceSubmittedError);
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
                .Equal(true)
                .WithMessage(r => TSVError);
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
                .Equal(true)
                .WithMessage(r => TSVError);
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
        User user = dataWorker.Users.GetByDiscordId(request.GetDiscordInteraction().User.Id)!;

        bool isValid = true;
        foreach (Tile tile in refreshedTiles)
        {
            if (tileValidator.Validate(tile, user, request.EvidenceType) is false)
            {
                isValid = false;
                AddTSVError(tile, tileValidator.ErrorMessage);
            }
        }

        return isValid;
    }

    private void AddTSVError(Tile tile, string error)
    {
        TSVError += $"{tile.Task.Name} error: {error}{Environment.NewLine}";
    }

    protected override IEnumerable<SemaphoreSlim> GetSemaphores(SubmitEvidenceSubmitButtonRequest request, RequestSemaphores semaphores) =>
        new List<SemaphoreSlim>()
        {
            semaphores.GetTeamDatabase(request.DiscordTeam.Id),
            semaphores.GetEvidence(request.DiscordTeam.Id)
        };
}