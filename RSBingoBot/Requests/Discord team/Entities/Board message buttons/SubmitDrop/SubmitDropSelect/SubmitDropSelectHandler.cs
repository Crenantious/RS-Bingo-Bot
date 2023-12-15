// <copyright file="SubmitDropSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.RequestHandlers;
using RSBingo_Framework.Models;

internal class SubmitDropSelectHandler : SelectComponentHandler<SubmitDropSelectRequest>
{
    protected override void OnItemsSelected(IEnumerable<SelectComponentItem> items, SubmitDropSelectRequest request,
        CancellationToken cancellationToken)
    {
        request.DTO.Tiles = items.Select(i => (Tile)i.Value!);
    }
}