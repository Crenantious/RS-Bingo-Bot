// <copyright file="ChangeTilesTileSelect.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Factories;
using RSBingo_Framework.Models;

public class ChangeTilesTileSelect
{
    private const string NoTaskName = "No task";

    private readonly Team team;

    private Dictionary<int, SelectComponentItem> boardIndexToItem = new();

    public SelectComponent SelectComponent { get; }

    public ChangeTilesTileSelect(Team team, ChangeTilesButtonDTO dto)
    {
        this.team = team;
        SelectComponentFactory selectComponentFactory = (SelectComponentFactory)General.DI.GetService(typeof(SelectComponentFactory))!;
        SelectComponent = CreateSelectComponent(dto, selectComponentFactory);
    }

    public void Update(IEnumerable<int> updatedBoardIndexes)
    {
        UpdateItem(SelectComponent.SelectedItems.ElementAt(0));

        foreach (int index in updatedBoardIndexes)
        {
            UpdateItem(boardIndexToItem[index]);
        }

        SelectComponent.Build();
    }

    private void UpdateItem(SelectComponentItem item)
    {
        int boardIndex = (int)item.Value!;
        Tile? tile = team.Tiles.FirstOrDefault(t => t.BoardIndex == boardIndex);
        item.Label = GetItemName(boardIndex, tile is null ? null : tile.Task);
    }

    private SelectComponent CreateSelectComponent(ChangeTilesButtonDTO dto, SelectComponentFactory selectComponentFactory) =>
        selectComponentFactory.Create(new(new SelectComponentPage("Change from", GetOptions(),
            SelectComponentGetPageName.CustomMethod(GetPageName))), () => new ChangeTilesFromSelectRequest(dto));

    private IEnumerable<SelectComponentOption> GetOptions()
    {
        List<SelectComponentItem> items = new();
        Dictionary<int, Tile> tiles = team.Tiles.ToDictionary(t => t.BoardIndex);
        BingoTask? task;

        for (int boardIndex = 0; boardIndex < General.MaxTilesOnABoard; boardIndex++)
        {
            task = tiles.ContainsKey(boardIndex) ? tiles[boardIndex].Task : null;
            items.Add(new(GetItemName(boardIndex, task), boardIndex));
            boardIndexToItem.Add(boardIndex, items[^1]);
        }

        return items;
    }

    private string GetItemName(int boardIndex, BingoTask? task)
    {
        string name = task is null ? NoTaskName : task.Name;
        return $"Tile {boardIndex} - {name}";
    }

    private string GetPageName(SelectComponentPage page)
    {
        var firstPage = (SelectComponentItem)page.Options[0];
        var lastPage = (SelectComponentItem)page.Options[^1];
        return $"Tiles {firstPage.Value} - {lastPage.Value}";
    }
}