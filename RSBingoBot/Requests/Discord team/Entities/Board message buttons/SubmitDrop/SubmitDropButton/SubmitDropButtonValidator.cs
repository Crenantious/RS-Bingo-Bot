// <copyright file="SubmitDropButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;
using RSBingoBot.Requests.Validation;
using System.Collections.Generic;
using System.Threading;

internal class SubmitDropButtonValidator : BingoValidator<SubmitDropButtonRequest>
{
    // TODO: JR - get the button name from somewhere.
    private const string ActiveChangeTileInstanceError = "This cannot be used while the 'Change tile' button is being used.";

    public SubmitDropButtonValidator(RequestSemaphores semaphores)
    {
        // TODO: JR - check against the change tile request when it is made.
        ActiveRequestInstances<ChangeTilesButtonRequest>(
            (r, c) => r.DiscordTeam.Id == c.TeamId,
            ActiveChangeTileInstanceError,
            1);

        TeamHasTiles(r => r.DiscordTeam.Id);
    }

    protected override IEnumerable<SemaphoreSlim> GetSemaphores(SubmitDropButtonRequest request, RequestSemaphores semaphores) =>
        new List<SemaphoreSlim>()
        {
            semaphores.GetEvidence(request.DiscordTeam.Id)
        };
}