// <copyright file="SubmitDropButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentValidation;
using RSBingoBot.Requests.Validation;

internal class SubmitDropButtonValidator : Validator<SubmitDropButtonRequest>
{
    private const string NoTilesError = "Your team has no tiles to submit evidence for.";

    public SubmitDropButtonValidator()
    {
        RuleFor(r => r.DiscordTeam.Team.Tiles.Any())
            .Equal(true)
            .WithMessage(NoTilesError);
    }
}