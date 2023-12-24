// <copyright file="JoinTeamSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Discord;

internal class JoinTeamSelectHandler : SelectComponentHandler<JoinTeamSelectRequest>
{
    private readonly IDiscordTeamServices teamServices;
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public JoinTeamSelectHandler(IDiscordTeamServices discordServices)
    {
        this.teamServices = discordServices;
    }

    protected override async Task OnItemSelectedAsync(IEnumerable<SelectComponentItem> items, JoinTeamSelectRequest request, CancellationToken cancellationToken)
    {
        DiscordTeam discordTeam = (DiscordTeam)items.ElementAt(0).Value!;
        Result addToTeam = await teamServices.AddUserToTeam(request.InteractionArgs.Interaction.User, discordTeam);
        AddResponses(addToTeam);
    }
}