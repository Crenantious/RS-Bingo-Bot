// <copyright file="ChangeTilesSubmitButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

internal class ChangeTilesSubmitButtonHandler : ButtonHandler<ChangeTilesSubmitButtonRequest>
{
    protected override async Task Process(ChangeTilesSubmitButtonRequest request, CancellationToken cancellationToken)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        Team team = dataWorker.Teams.GetTeamByID(request.TeamId)!;
        Tile? tile1 = team.Tiles.FirstOrDefault(t => t.BoardIndex == request.DTO.TileBoardIndex);
        Tile? tile2 = team.Tiles.FirstOrDefault(t => t.Task == request.DTO.Task);

        UpdateTiles(request, dataWorker, team, tile1, tile2);
        dataWorker.SaveChanges();
    }

    private void UpdateTiles(ChangeTilesSubmitButtonRequest request, IDataWorker dataWorker, Team team, Tile? tile1, Tile? tile2)
    {
        if (tile1 is null)
        {
            if (tile2 is null)
            {
                Tile newTile = dataWorker.Tiles.Create(team, request.DTO.Task!, (int)request.DTO.TileBoardIndex!);
                AddSuccess(new ChangeTilesSubmitButtonAddedTileToBoardSuccess(newTile));
                return;
            }

            tile2.BoardIndex = (int)request.DTO.TileBoardIndex!;
            AddSuccess(new ChangeTilesSubmitButtonMoveTileOnBoardSuccess(tile2));
            return;
        }

        if (tile2 is null)
        {
            tile1.Task = request.DTO.Task!;
            AddSuccess(new ChangeTilesSubmitButtonAddedTaskToBoardSuccess(tile1.Task));
            return;
        }

        tile1.SwapTasks(tile2, dataWorker);
        AddSuccess(new ChangeTilesSubmitButtonSwappedTilesSuccess(tile1, tile2));
    }
}