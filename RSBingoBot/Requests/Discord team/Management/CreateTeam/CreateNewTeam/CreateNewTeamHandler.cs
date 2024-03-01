// <copyright file="CreateNewTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

internal class CreateNewTeamHandler : RequestHandler<CreateNewTeamRequest, DiscordTeam>
{
    protected override async Task<DiscordTeam> Process(CreateNewTeamRequest request, CancellationToken cancellationToken)
    {
        var discordServices = GetRequestService<IDiscordTeamServices>();
        var dbServices = GetRequestService<IDatabaseServices>();

        Team team = request.DataWorker.Teams.Create();
        team.Name = request.Name;
        await dbServices.SaveChanges(request.DataWorker);

        DiscordTeam discordTeam = new(team);

        Result result = await discordServices.CreateMissingEntities(discordTeam, request.DataWorker);
        if (result.IsSuccess)
        {
            DiscordTeam.ExistingTeams.Add(request.Name, discordTeam);
            AddSuccess(new CreateNewTeamSuccess(request.Name));
        }
        else
        {
            request.DataWorker.Teams.Remove(team);
            AddError(new CreateNewTeamInitialisationError());
        }
        await dbServices.SaveChanges(request.DataWorker);

        return discordTeam;
    }
}