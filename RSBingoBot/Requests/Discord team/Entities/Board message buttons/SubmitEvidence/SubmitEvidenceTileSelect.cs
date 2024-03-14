// <copyright file="SubmitEvidenceTileSelect.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Factories;
using DSharpPlus.Entities;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using static RSBingo_Framework.Records.EvidenceRecord;

internal class SubmitEvidenceTileSelect
{
    private readonly IDataWorker dataWorker;
    private readonly SubmitEvidenceButtonDTO dto;
    private readonly User user;
    private readonly EvidenceType evidenceType;
    private readonly IEvidenceVerificationEmojis evidenceVerificationEmojis;
    private readonly SubmitEvidenceTSV submitEvidenceTSV;
    private Dictionary<int, SelectComponentItem> tileIdToItem = new();

    public SelectComponent SelectComponent { get; }

    public SubmitEvidenceTileSelect(IDataWorker dataWorker, SubmitEvidenceButtonDTO dto, User user, EvidenceType evidenceType,
        IEvidenceVerificationEmojis evidenceVerificationEmojis)
    {
        this.dataWorker = dataWorker;
        this.dto = dto;
        this.user = user;
        this.evidenceType = evidenceType;
        this.evidenceVerificationEmojis = evidenceVerificationEmojis;
        this.submitEvidenceTSV = General.DI.Get<SubmitEvidenceTSV>();

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
            () => new SubmitEvidenceSelectRequest(dto));

    private IEnumerable<SelectComponentItem> GetItems()
    {
        List<SelectComponentItem> items = new();
        var tiles = user.Team.Tiles
               .Where(t => submitEvidenceTSV.Validate(t, user, evidenceType))
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

        DiscordEmoji? discordEmoji = evidenceVerificationEmojis.GetStatusEmoji(evidence);
        return discordEmoji is null ? null : new DiscordComponentEmoji(discordEmoji);
    }

    private Evidence? GetEvidenceForEmoji(Tile tile) =>
        evidenceType switch
        {
            EvidenceType.Drop => GetFirstPendingDropEvidence(tile),
            EvidenceType.TileVerification => GetByTileUserAndType(dataWorker, tile, user, evidenceType),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static Evidence? GetFirstPendingDropEvidence(Tile tile) =>
        tile.Evidence.FirstOrDefault(e =>
            EvidenceTypeLookup.Get(e.EvidenceType) == EvidenceType.Drop &&
            EvidenceStatusLookup.Get(e.Status) == EvidenceStatus.PendingReview);
}