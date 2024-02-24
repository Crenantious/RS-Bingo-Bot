// <copyright file="ChangeTilesSubmitButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;
using RSBingoBot.Imaging;

internal class ChangeTilesSubmitButtonHandler : ButtonHandler<ChangeTilesSubmitButtonRequest>
{
    protected override async Task Process(ChangeTilesSubmitButtonRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();

        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        Team team = dataWorker.Teams.GetTeamByID(request.TeamId)!;
        Tile? tile1 = team.Tiles.FirstOrDefault(t => t.BoardIndex == request.DTO.ChangeFromTileBoardIndex);
        Tile? tile2 = team.Tiles.FirstOrDefault(t => t.Task == request.DTO.ChangeToTask);

        var updatedTiles = UpdateTiles(request, dataWorker, team, tile1, tile2);
        Image board = BoardImage.UpdateTiles(team, updatedTiles);
        BoardImage.SaveBoard(board, team.Name);

        dataWorker.SaveChanges();

        await messageServices.Update(DiscordTeam.ExistingTeams[team.Name].BoardMessage!);
    }

    private List<Tile> UpdateTiles(ChangeTilesSubmitButtonRequest request, IDataWorker dataWorker, Team team, Tile? tile1, Tile? tile2)
    {
        List<Tile> updatedTiles = new();

        if (tile1 is null)
        {
            if (tile2 is null)
            {
                Tile newTile = dataWorker.Tiles.Create(team, request.DTO.ChangeToTask!, (int)request.DTO.ChangeFromTileBoardIndex!);
                updatedTiles.Add(newTile);
                AddSuccess(new ChangeTilesSubmitButtonAddedTileToBoardSuccess(newTile));
                return updatedTiles;
            }

            tile2.BoardIndex = (int)request.DTO.ChangeFromTileBoardIndex!;
            updatedTiles.Add(tile2);
            AddSuccess(new ChangeTilesSubmitButtonMoveTileOnBoardSuccess(tile2));
            return updatedTiles;
        }

        if (tile2 is null)
        {
            tile1.Task = request.DTO.ChangeToTask!;
            updatedTiles.Add(tile1);
            AddSuccess(new ChangeTilesSubmitButtonAddedTaskToBoardSuccess(tile1.Task));
            return updatedTiles;
        }

        tile1.SwapTasks(tile2, dataWorker);
        updatedTiles.Add(tile1);
        updatedTiles.Add(tile2);
        AddSuccess(new ChangeTilesSubmitButtonSwappedTilesSuccess(tile1, tile2));
        return updatedTiles;
    }
}