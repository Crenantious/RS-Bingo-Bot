// <copyright file="ChangeTilesButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Discord;
using RSBingoBot.Requests.Validation;

internal class ChangeTilesButtonValidator : BingoValidator<ChangeTilesButtonRequest>
{
    public ChangeTilesButtonValidator()
    {
        ClassLevelCascadeMode = FluentValidation.CascadeMode.Stop;

        ActiveInteractions<ChangeTilesButtonRequest>((r, t) => r.TeamId == t.Request.TeamId,
            GetTooManyInteractionInstancesError(1, DiscordTeamBoardButtons.ChangeTileLabel), 1);

        ActiveInteractions<SubmitEvidenceButtonRequest>((r, t) => r.TeamId == t.Request.DiscordTeam.Id,
            DiscordTeamBoardButtonErrors.ChangeTilesWithActiveSubmitDropOrViewEvidence, 1);

        ActiveInteractions<ViewEvidenceButtonRequest>((r, t) => r.TeamId == t.Request.TeamId,
            DiscordTeamBoardButtonErrors.ChangeTilesWithActiveSubmitDropOrViewEvidence, 1);

        TeamExists(r => r.TeamId);
        UserOnTeam(r => (r.GetDiscordInteraction().User, r.TeamId), true);
    }
}