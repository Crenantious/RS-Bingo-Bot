// <copyright file="SubmitDropButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentValidation;
using RSBingoBot.Requests.Validation;

internal class SubmitDropButtonValidator : BingoValidator<SubmitDropButtonRequest>
{
    // TODO: JR - get the button name from somewhere.
    private const string ActiveChangeTileInstanceError = "This cannot be used while the 'Change tile' button is being used.";
    private const string NoTilesError = "Your team has no tiles to submit evidence for.";

    public SubmitDropButtonValidator(RequestSemaphores semaphores)
    {
        SetSemaphores(semaphores.UpdateEvidence);

        // TODO: JR - check against the change tile request when it is made.
        RequestHandlerInstanceExists<ChangeTilesButtonRequest>((r, c) => r.DiscordTeam.Team.RowId == c.TeamId, ActiveChangeTileInstanceError);
        RuleFor(r => r.DiscordTeam.Team.Tiles.Any())
            .Equal(true)
            .WithMessage(NoTilesError);
    }
}