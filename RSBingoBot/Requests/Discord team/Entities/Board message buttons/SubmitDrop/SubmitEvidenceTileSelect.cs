// <copyright file="SubmitEvidenceTileSelect.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Factories;
using DSharpPlus.Entities;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class SubmitEvidenceTileSelect
{
    private readonly IDataWorker dataWorker;
    private readonly SubmitDropButtonDTO dto;
    private readonly User user;
    private readonly EvidenceRecord.EvidenceType evidenceType;

    private Dictionary<int, SelectComponentItem> tileIdToItem = new();

    public SelectComponent SelectComponent { get; }

    public SubmitEvidenceTileSelect(IDataWorker dataWorker, SubmitDropButtonDTO dto, User user, EvidenceRecord.EvidenceType evidenceType)
    {
        this.dataWorker = dataWorker;
        this.dto = dto;
        this.user = user;
        this.evidenceType = evidenceType;

        SelectComponentFactory selectComponentFactory = (SelectComponentFactory)General.DI.GetService(typeof(SelectComponentFactory))!;
        SelectComponent = CreateSelectComponent(selectComponentFactory);
    }

    public void Update(IEnumerable<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            var item = tileIdToItem[tile.RowId];
            item.Emoji = GetSelectOptionEmoji(tile);
        }

        SelectComponent.Build();
    }

    private SelectComponent CreateSelectComponent(SelectComponentFactory selectComponentFactory) =>
        selectComponentFactory.Create(
            new(new SelectComponentPage("Select a tile", GetItems()), MaxOptions: General.MaxSelectOptionsPerPage),
            () => new SubmitDropSelectRequest(dto));

    private IEnumerable<SelectComponentItem> GetItems()
    {
        List<SelectComponentItem> items = new();
        var tiles = user.Team.Tiles
               .Where(t => t.IsCompleteAsBool() is false)
               .OrderBy(t => t.BoardIndex);

        foreach (var tile in tiles)
        {
            items.Add(CreateItem(tile));
        }
        return items;

    }
    private SelectComponentItem CreateItem(Tile tile)
    {
        SelectComponentItem item = new(tile.Task.Name, tile, emoji: GetSelectOptionEmoji(tile));
        tileIdToItem.Add(tile.RowId, item);
        return item;
    }

    private DiscordComponentEmoji? GetSelectOptionEmoji(Tile tile)
    {
        Evidence? evidence = GetEvidenceForEmoji(tile);
        if (evidence == null)
        {
            return null;
        }

        DiscordEmoji? discordEmoji = BingoBotCommon.GetEvidenceStatusEmoji(evidence);
        return discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
    }

    private Evidence? GetEvidenceForEmoji(Tile tile) =>
        evidenceType switch
        {
            EvidenceRecord.EvidenceType.Drop => GetFirstDropEvidence(tile),
            EvidenceRecord.EvidenceType.TileVerification => EvidenceRecord.GetByTileUserAndType(dataWorker, tile, user, evidenceType),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static Evidence? GetFirstDropEvidence(Tile tile) =>
        tile.Evidence.FirstOrDefault(e =>
            EvidenceRecord.EvidenceTypeLookup.Get(e.EvidenceType) == EvidenceRecord.EvidenceType.Drop &&
            EvidenceRecord.EvidenceStatusLookup.Get(e.Status) != EvidenceRecord.EvidenceStatus.Rejected);

    //private string GetItemName(int boardIndex, BingoTask? task)
    //{
    //    string name = task is null ? NoTaskName : task.Name;
    //    return $"Tile {boardIndex} - {name}";
    //}

    //private string GetPageName(SelectComponentPage page)
    //{
    //    var firstPage = (SelectComponentItem)page.Options[0];
    //    var lastPage = (SelectComponentItem)page.Options[^1];
    //    return $"Tiles {firstPage.Value} - {lastPage.Value}";
    //}
}