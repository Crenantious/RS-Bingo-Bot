// <copyright file="JoinTeamSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Discord;

internal class JoinTeamSelectHandler : SelectComponentHandler<JoinTeamSelectRequest>
{
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    protected override async Task OnItemSelectedAsync(IEnumerable<SelectComponentItem> items, JoinTeamSelectRequest request, CancellationToken cancellationToken)
    {
        var teamServices = GetRequestService<IDiscordTeamServices>();
        var dbServices = GetRequestService<IDatabaseServices>();

        DiscordTeam discordTeam = (DiscordTeam)items.ElementAt(0).Value!;
        Result addToTeam = await teamServices.AddUserToTeam(request.GetDiscordInteraction().User, discordTeam, dataWorker);
        await dbServices.Update(dataWorker);
    }
}