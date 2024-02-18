// <copyright file="SubmitDropButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Discord;
using RSBingoBot.Requests.Validation;

internal class SubmitDropButtonValidator : BingoValidator<SubmitDropButtonRequest>
{
    public SubmitDropButtonValidator(RequestSemaphores semaphores)
    {
        ClassLevelCascadeMode = FluentValidation.CascadeMode.Stop;

        ActiveInteractions<ChangeTilesButtonRequest>((r, t) => r.DiscordTeam.Id == t.Request.TeamId,
              DiscordTeamBoardButtonErrors.SubmitDropOrViewEvidenceWithActiveChangeTiles, 1);

        // TODO: JR - fix the label name.
        ActiveInteractions<SubmitDropButtonRequest>((r, t) => r.DiscordTeam.Id == t.Request.DiscordTeam.Id,
            GetTooManyInteractionInstancesError(1, DiscordTeamBoardButtons.SubmitEvidenceLabel), 1);

        ActiveInteractions<ViewEvidenceButtonRequest>((r, t) => r.DiscordTeam.Id == t.Request.TeamId,
            DiscordTeamBoardButtonErrors.SubmitDropWithActiveViewEvidence, 1);

        TeamExists(r => r.DiscordTeam.Id);
        UserOnTeam(r => (r.GetDiscordInteraction().User, r.DiscordTeam.Id), true);
        TeamHasTiles(r => r.DiscordTeam.Id);
    }
}