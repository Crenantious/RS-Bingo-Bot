// <copyright file="DeleteTeamCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using RSBingoBot.Discord;

internal class DeleteTeamCommandHandler : CommandHandler<DeleteTeamCommandRequest>
{
    protected override async Task Process(DeleteTeamCommandRequest request, CancellationToken cancellationToken)
    {
        var teamServices = GetRequestService<IDiscordTeamServices>();
        await teamServices.Delete(DiscordTeam.ExistingTeams[request.TeamName]);
    }
}