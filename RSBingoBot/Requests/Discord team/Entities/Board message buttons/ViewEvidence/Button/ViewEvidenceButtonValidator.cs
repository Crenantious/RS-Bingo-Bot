// <copyright file="ViewEvidenceButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Requests.Extensions;
using FluentValidation;
using RSBingoBot.Discord;
using RSBingoBot.Requests;

internal class ViewEvidenceButtonValidator : BingoValidator<ViewEvidenceButtonRequest>
{
    private const string NoTilesFoundError = "You have not submitted evidence for any tiles.";

    public ViewEvidenceButtonValidator()
    {
        ClassLevelCascadeMode = FluentValidation.CascadeMode.Stop;

        ActiveInteractions<ChangeTilesButtonRequest>((r, t) => r.TeamId == t.Request.TeamId,
            DiscordTeamBoardButtonErrors.SubmitDropOrViewEvidenceWithActiveChangeTiles, 1);

        // TODO: JR - fix the label name.
        ActiveInteractions<SubmitEvidenceButtonRequest>((r, t) => r.TeamId == t.Request.DiscordTeam.Id,
            DiscordTeamBoardButtonErrors.ViewEvidenceWithActiveSubmitDrop, 1);

        ActiveInteractions<ViewEvidenceButtonRequest>((r, t) => r.TeamId == t.Request.TeamId,
            GetTooManyInteractionInstancesError(1, DiscordTeamBoardButtons.ViewEvidenceLabel), 1);

        TeamExists(r => r.TeamId);
        UserOnTeam(r => (r.GetDiscordInteraction().User, r.TeamId), true);
        RuleFor(r => r.GetDiscordInteraction().User.GetDBUser(DataWorker)!.Evidence)
            .NotEmpty()
            .WithMessage(NoTilesFoundError);
    }
}