// <copyright file="ViewEvidenceButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Requests.Validation;
using FluentValidation;
using RSBingoBot.Requests;

internal class ViewEvidenceButtonValidator : Validator<ViewEvidenceButtonRequest>
{
    private const string NoTilesFoundError = "You have not submitted evidence for any tiles.";

    public ViewEvidenceButtonValidator()
    {
        TeamExists(r => r.Team);
        UserOnTeam(r => (r.InteractionArgs.Interaction.User, r.Team));
        RuleFor(r => r.InteractionArgs.Interaction.User.GetDBUser(DataWorker)!.Evidence)
            .NotEmpty()
            .WithMessage(NoTilesFoundError);
    }
}