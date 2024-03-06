// <copyright file="ChangeTilesTileSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;

internal class ChangeTilesTileSelectHandler : SelectComponentHandler<ChangeTilesTileSelectRequest>
{
    protected override void OnItemsSelected(IEnumerable<SelectComponentItem> items, ChangeTilesTileSelectRequest request, CancellationToken cancellationToken)
    {
        request.DTO.TileBoardIndex = (int)items.First().Value!;
    }
}