// <copyright file="SubmitEvidenceButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Discord;
using RSBingoBot.Requests.Validation;

internal class SubmitEvidenceButtonValidator : BingoValidator<SubmitEvidenceButtonRequest>
{
    public SubmitEvidenceButtonValidator(RequestSemaphores semaphores)
    {
        ClassLevelCascadeMode = FluentValidation.CascadeMode.Stop;

        ActiveInteractions<ChangeTilesButtonRequest>((r, t) => r.DiscordTeam.Id == t.Request.TeamId,
              DiscordTeamBoardButtonErrors.SubmitDropOrViewEvidenceWithActiveChangeTiles, 1);

        // TODO: JR - fix the label name.
        ActiveInteractions<SubmitEvidenceButtonRequest>((r, t) => r.DiscordTeam.Id == t.Request.DiscordTeam.Id,
            GetTooManyInteractionInstancesError(1, DiscordTeamBoardButtons.SubmitEvidenceLabel), 1);

        ActiveInteractions<ViewEvidenceButtonRequest>((r, t) => r.DiscordTeam.Id == t.Request.TeamId,
            DiscordTeamBoardButtonErrors.SubmitDropWithActiveViewEvidence, 1);

        TeamExists(r => r.DiscordTeam.Id);
        UserOnTeam(r => (r.GetDiscordInteraction().User, r.DiscordTeam.Id), true);
    }
}