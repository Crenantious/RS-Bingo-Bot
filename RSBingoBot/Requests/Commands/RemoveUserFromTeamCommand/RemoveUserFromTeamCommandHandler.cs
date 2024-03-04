// <copyright file="RemoveUserFromTeamCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using DiscordTeam = RSBingoBot.Discord.DiscordTeam;

internal class RemoveUserFromTeamCommandHandler : CommandHandler<RemoveUserFromTeamCommandRequest>
{
    protected override async Task Process(RemoveUserFromTeamCommandRequest request, CancellationToken cancellationToken)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        User user = dataWorker.Users.FirstOrDefault(u => u.DiscordUserId == request.Member.Id)!;
        var team = DiscordTeam.ExistingTeams[user.Team.Name];

        var discordServices = GetRequestService<IDiscordServices>();

        var teamServices = GetRequestService<IDiscordTeamServices>();
        await teamServices.RemoveUserFromTeam(dataWorker, request.Member, user, team);
    }
}