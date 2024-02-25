﻿// <copyright file="ChangeTilesSubmitButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;
using RSBingoBot.Imaging;

internal class ChangeTilesSubmitButtonHandler : ButtonHandler<ChangeTilesSubmitButtonRequest>
{
    protected override async Task Process(ChangeTilesSubmitButtonRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();

        Tile? tile1 = request.Team.Tiles.FirstOrDefault(t => t.BoardIndex == request.DTO.ChangeFromTileBoardIndex);
        Tile? tile2 = request.Team.Tiles.FirstOrDefault(t => t.Task.RowId == request.DTO.ChangeToTask.RowId);

        var updatedTiles = UpdateTiles(request, tile1, tile2);
        Image board = BoardImage.UpdateTiles(request.Team, updatedTiles);
        BoardImage.SaveBoard(board, request.Team.Name);

        request.DataWorker.SaveChanges();

        await messageServices.Update(DiscordTeam.ExistingTeams[request.Team.Name].BoardMessage!);
    }

    private List<Tile> UpdateTiles(ChangeTilesSubmitButtonRequest request, Tile? tile1, Tile? tile2)
    {
        List<Tile> updatedTiles = new();

        if (tile1 is null)
        {
            if (tile2 is null)
            {
                Tile newTile = request.DataWorker.Tiles.Create(request.Team, request.DTO.ChangeToTask!, (int)request.DTO.ChangeFromTileBoardIndex!);
                updatedTiles.Add(newTile);
                AddSuccess(new ChangeTilesSubmitButtonAddedTileToBoardSuccess(newTile));
                return updatedTiles;
            }

            int oldBoardIndex = tile2.BoardIndex;
            tile2.BoardIndex = (int)request.DTO.ChangeFromTileBoardIndex!;
            updatedTiles.Add(tile2);
            AddSuccess(new ChangeTilesSubmitButtonMoveTileOnBoardSuccess(tile2, oldBoardIndex, tile2.BoardIndex));
            return updatedTiles;
        }

        if (tile2 is null)
        {
            BingoTask oldTask = tile1.Task;
            tile1.Task = request.DTO.ChangeToTask!;
            updatedTiles.Add(tile1);
            AddSuccess(new ChangeTilesSubmitButtonAddedTaskToBoardSuccess(oldTask, tile1.Task));
            return updatedTiles;
        }

        tile1.SwapTasks(tile2, request.DataWorker);
        updatedTiles.Add(tile1);
        updatedTiles.Add(tile2);
        AddSuccess(new ChangeTilesSubmitButtonSwappedTilesSuccess(tile1, tile2));
        return updatedTiles;
    }
}