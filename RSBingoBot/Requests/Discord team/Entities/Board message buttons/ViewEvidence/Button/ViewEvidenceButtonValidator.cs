// <copyright file="ViewEvidenceButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Requests.Extensions;
using FluentValidation;
using RSBingoBot.Requests;

internal class ViewEvidenceButtonValidator : BingoValidator<ViewEvidenceButtonRequest>
{
    private const string NoTilesFoundError = "You have not submitted evidence for any tiles.";

    public ViewEvidenceButtonValidator()
    {
        TeamExists(r => r.Team);
        UserOnTeam(r => (r.GetDiscordInteraction().User, r.Team));
        RuleFor(r => r.GetDiscordInteraction().User.GetDBUser(DataWorker)!.Evidence)
            .NotEmpty()
            .WithMessage(NoTilesFoundError);
    }
}