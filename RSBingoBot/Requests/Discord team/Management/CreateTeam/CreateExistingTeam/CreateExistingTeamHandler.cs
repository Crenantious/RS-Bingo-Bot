﻿// <copyright file="CreateExistingTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using FluentResults;
using RSBingoBot.Discord;

internal class CreateExistingTeamHandler : RequestHandler<CreateExistingTeamRequest, DiscordTeam>
{
    protected override async Task<DiscordTeam> Process(CreateExistingTeamRequest request, CancellationToken cancellationToken)
    {
        var teamServices = GetRequestService<IDiscordTeamServices>();

        DiscordTeam discordTeam = new(request.Team);

        Result result = await teamServices.SetExistingEntities(discordTeam);

        await teamServices.UpdateBoardImage(discordTeam, request.Team, GetTilesForBoard(request));

        return discordTeam;
    }

    private static IEnumerable<int> GetTilesForBoard(CreateExistingTeamRequest request) =>
        request.Team.Tiles.Select(t => t.BoardIndex);
}