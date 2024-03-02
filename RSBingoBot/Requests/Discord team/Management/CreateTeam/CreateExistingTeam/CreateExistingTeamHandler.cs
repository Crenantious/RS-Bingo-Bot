// <copyright file="CreateExistingTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using FluentResults;
using Imaging.Board;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

internal class CreateExistingTeamHandler : RequestHandler<CreateExistingTeamRequest, DiscordTeam>
{
    protected override async Task<DiscordTeam> Process(CreateExistingTeamRequest request, CancellationToken cancellationToken)
    {
        var teamServices = GetRequestService<IDiscordTeamServices>();

        DiscordTeam discordTeam = new(request.Team);

        discordTeam.Board.UpdateTiles(GetTilesForBoard(request));

        Result result = await teamServices.SetExistingEntities(discordTeam);

        return discordTeam;
    }

    private static IEnumerable<(BingoTask?, int)> GetTilesForBoard(CreateExistingTeamRequest request) =>
        request.Team.Tiles.Select<Tile, (BingoTask?, int)>(t => (t.Task, t.BoardIndex));
}