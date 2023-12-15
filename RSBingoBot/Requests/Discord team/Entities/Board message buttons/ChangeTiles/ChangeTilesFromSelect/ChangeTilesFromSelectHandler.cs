// <copyright file="ChangeTilesFromSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.RequestHandlers;

internal class ChangeTilesFromSelectHandler : SelectComponentHandler<ChangeTilesFromSelectRequest>
{
    protected override void OnItemsSelected(IEnumerable<SelectComponentItem> items, ChangeTilesFromSelectRequest request, CancellationToken cancellationToken)
    {
        request.DTO.TileBoardIndex = (int)items.First().Value!;
    }
}