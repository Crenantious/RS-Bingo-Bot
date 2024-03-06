// <copyright file="ChangeTilesTaskSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using RSBingo_Framework.Models;

internal class ChangeTilesTaskSelectHandler : SelectComponentHandler<ChangeTilesTaskSelectRequest>
{
    protected override void OnItemsSelected(IEnumerable<SelectComponentItem> items, ChangeTilesTaskSelectRequest request, CancellationToken cancellationToken)
    {
        request.DTO.Task = (BingoTask)items.First().Value!;
    }
}