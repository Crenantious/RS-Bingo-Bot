// <copyright file="AddUserToTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;
using System.Collections.Generic;

internal class AddUserToTeamValidator : BingoValidator<AddUserToTeamRequest>
{
    public AddUserToTeamValidator(RequestSemaphores semaphores)
    {
        UserNotNull(r => r.User);
        UserNotOnATeam(r => r.User, true);
    }

    protected override IEnumerable<SemaphoreSlim> GetSemaphores(AddUserToTeamRequest request, RequestSemaphores semaphores) =>
        new List<SemaphoreSlim>()
        {
            semaphores.GetTeamDatabase(request.DiscordTeam.Id)
        };
}