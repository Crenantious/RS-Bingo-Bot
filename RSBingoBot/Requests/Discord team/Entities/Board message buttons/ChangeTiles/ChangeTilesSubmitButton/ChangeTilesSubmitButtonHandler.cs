// <copyright file="ChangeTilesSubmitButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;

// TODO: JR - add validation for difficulty based on the board index.
internal class ChangeTilesSubmitButtonHandler : ButtonHandler<ChangeTilesSubmitButtonRequest>
{
    private List<int> updatedBoardIndexes = new();
    private List<BingoTask> updatedTasks = new();

    protected override async Task Process(ChangeTilesSubmitButtonRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();
        var discordTeam = RSBingoBot.Discord.DiscordTeam.ExistingTeams[request.Team.Name];

        Tile? tile1 = request.Team.Tiles.FirstOrDefault(t => t.BoardIndex == request.DTO.TileBoardIndex);
        Tile? tile2 = request.Team.Tiles.FirstOrDefault(t => t.Task.RowId == request.DTO.Task!.RowId);

        UpdateDB(request, tile1, tile2);
        request.DataWorker.SaveChanges();

        UpdateSelectComponents(request);
        UpdateBoardImage(request, discordTeam);

        await messageServices.Update(request.ChangeTilesTileSelect.SelectComponent.Message!);
    }

    private void UpdateDB(ChangeTilesSubmitButtonRequest request, Tile? tile1, Tile? tile2)
    {
        if (tile1 is null)
        {
            if (tile2 is null)
            {
                Tile newTile = request.DataWorker.Tiles.Create(request.Team, request.DTO.Task!, (int)request.DTO.TileBoardIndex!);
                updatedBoardIndexes.Add(newTile.BoardIndex);
                updatedTasks.Add(newTile.Task);
                AddSuccess(new ChangeTilesSubmitButtonAddedTileToBoardSuccess(newTile));
                return;
            }

            int oldBoardIndex = tile2.BoardIndex;
            tile2.BoardIndex = (int)request.DTO.TileBoardIndex!;
            updatedBoardIndexes.Add(oldBoardIndex);
            updatedBoardIndexes.Add(tile2.BoardIndex);
            AddSuccess(new ChangeTilesSubmitButtonMoveTileOnBoardSuccess(tile2, oldBoardIndex, tile2.BoardIndex));
            return;
        }

        if (tile2 is null)
        {
            BingoTask oldTask = tile1.Task;
            tile1.Task = request.DTO.Task!;
            updatedBoardIndexes.Add(tile1.BoardIndex);
            updatedTasks.Add(oldTask);
            updatedTasks.Add(tile1.Task);
            AddSuccess(new ChangeTilesSubmitButtonAddedTaskToBoardSuccess(oldTask, tile1.Task));
            return;
        }

        tile1.SwapTasks(tile2, request.DataWorker);
        updatedBoardIndexes.Add(tile1.BoardIndex);
        updatedBoardIndexes.Add(tile2.BoardIndex);
        AddSuccess(new ChangeTilesSubmitButtonSwappedTilesSuccess(tile1, tile2));
        return;
    }

    private void UpdateSelectComponents(ChangeTilesSubmitButtonRequest request)
    {
        Dictionary<int, Tile> boardIndexToTile = request.Team.Tiles.ToDictionary(t => t.BoardIndex);
        request.ChangeTilesTileSelect.Update(updatedBoardIndexes);
        request.ChangeTilesTaskSelect.Update(updatedTasks);
    }

    private async void UpdateBoardImage(ChangeTilesSubmitButtonRequest request, DiscordTeam discordTeam)
    {
        var teamServices = GetRequestService<IDiscordTeamServices>();
        await teamServices.UpdateBoardImage(discordTeam, request.Team, updatedBoardIndexes);
    }
}