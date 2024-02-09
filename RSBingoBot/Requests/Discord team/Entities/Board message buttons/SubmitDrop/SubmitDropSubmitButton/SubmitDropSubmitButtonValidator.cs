// <copyright file="SubmitDropSubmitButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentValidation;
using RSBingoBot.Requests.Validation;
using System.Collections.Generic;
using System.Threading;

internal class SubmitDropSubmitButtonValidator : BingoValidator<SubmitDropSubmitButtonRequest>
{
    private const string NoTilesSelectedError = "At least one tile must be selected to submit evidence for.";
    private const string NoEvidenceSubmittedError = "You cannot submit no evidence; please post a message with a single image first.";

    public SubmitDropSubmitButtonValidator()
    {
        RuleFor(r => r.DTO.Tiles.Any())
            .Equal(true)
            .WithMessage(NoTilesSelectedError);
        NotNull(r => r.DTO.EvidenceUrl, NoEvidenceSubmittedError);
    }

    protected override IEnumerable<SemaphoreSlim> GetSemaphores(SubmitDropSubmitButtonRequest request, RequestSemaphores semaphores) =>
        new List<SemaphoreSlim>()
        {
            semaphores.GetTeamDatabase(request.DiscordTeam.Id),
            semaphores.GetEvidence(request.DiscordTeam.Id)
        };
}