// <copyright file="DeleteTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;
using System.Collections.Generic;
using System.Threading;

internal class DeleteTeamValidator : BingoValidator<DeleteTeamRequest>
{
    public DeleteTeamValidator()
    {
        TeamExists(r => r.DiscordTeam.Id);
    }

    protected override IEnumerable<SemaphoreSlim> GetSemaphores(DeleteTeamRequest request, RequestSemaphores semaphores) =>
        new List<SemaphoreSlim>()
        {
            semaphores.GetTeamDatabase(request.DiscordTeam.Id)
        };
}