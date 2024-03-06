// <copyright file="ChangeTilesSubmitButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using Imaging.Board;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;

// TODO: JR - add validation for difficulty based on the board index.
internal class ChangeTilesSubmitButtonHandler : ButtonHandler<ChangeTilesSubmitButtonRequest>
{
    protected override async Task Process(ChangeTilesSubmitButtonRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();
        var discordTeam = RSBingoBot.Discord.DiscordTeam.ExistingTeams[request.Team.Name];

        Tile? tile1 = request.Team.Tiles.FirstOrDefault(t => t.BoardIndex == request.DTO.TileBoardIndex);
        Tile? tile2 = request.Team.Tiles.FirstOrDefault(t => t.Task.RowId == request.DTO.Task!.RowId);

        var updatedTiles = UpdateDB(request, tile1, tile2);
        request.DataWorker.SaveChanges();

        UpdateSelectComponents(request, updatedTiles);
        UpdateBoardImage(request, updatedTiles, discordTeam);

        await messageServices.Update(discordTeam.BoardMessage!);
        await messageServices.Update(request.ChangeTilesTileSelect.SelectComponent.Message!);
    }

    private List<(BingoTask?, int)> UpdateDB(ChangeTilesSubmitButtonRequest request, Tile? tile1, Tile? tile2)
    {
        List<(BingoTask?, int)> updatedTiles = new();

        if (tile1 is null)
        {
            if (tile2 is null)
            {
                Tile newTile = request.DataWorker.Tiles.Create(request.Team, request.DTO.Task!, (int)request.DTO.TileBoardIndex!);
                updatedTiles.Add((newTile.Task, newTile.BoardIndex));
                AddSuccess(new ChangeTilesSubmitButtonAddedTileToBoardSuccess(newTile));
                return updatedTiles;
            }

            int oldBoardIndex = tile2.BoardIndex;
            tile2.BoardIndex = (int)request.DTO.TileBoardIndex!;
            updatedTiles.Add((null, oldBoardIndex));
            updatedTiles.Add((tile2.Task, tile2.BoardIndex));
            AddSuccess(new ChangeTilesSubmitButtonMoveTileOnBoardSuccess(tile2, oldBoardIndex, tile2.BoardIndex));
            return updatedTiles;
        }

        if (tile2 is null)
        {
            BingoTask oldTask = tile1.Task;
            tile1.Task = request.DTO.Task!;
            updatedTiles.Add((oldTask, tile1.BoardIndex));
            updatedTiles.Add((tile1.Task, tile1.BoardIndex));
            AddSuccess(new ChangeTilesSubmitButtonAddedTaskToBoardSuccess(oldTask, tile1.Task));
            return updatedTiles;
        }

        tile1.SwapTasks(tile2, request.DataWorker);
        updatedTiles.Add((tile1.Task, tile1.BoardIndex));
        updatedTiles.Add((tile2.Task, tile2.BoardIndex));
        AddSuccess(new ChangeTilesSubmitButtonSwappedTilesSuccess(tile1, tile2));
        return updatedTiles;
    }

    private static void UpdateSelectComponents(ChangeTilesSubmitButtonRequest request, List<(BingoTask?, int)> updatedTiles)
    {
        request.ChangeTilesTileSelect.Update(updatedTiles.Select(t => t.Item2));
        request.ChangeTilesTaskSelect.Update(updatedTiles.Where(t => t.Item1 is not null).Select(t => t.Item1!));
    }

    private static void UpdateBoardImage(ChangeTilesSubmitButtonRequest request, List<(BingoTask?, int)> updatedTiles, DiscordTeam discordTeam)
    {
        discordTeam.Board.UpdateTiles(updatedTiles);
        request.BoardMessageFile.SetContent(discordTeam.Board.Image, discordTeam.Board.FileExtension);
    }
}