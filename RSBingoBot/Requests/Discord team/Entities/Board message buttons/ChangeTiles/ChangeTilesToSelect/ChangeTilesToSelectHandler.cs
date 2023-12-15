// <copyright file="ChangeTilesToSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.RequestHandlers;
using RSBingo_Framework.Models;

internal class ChangeTilesToSelectHandler : SelectComponentHandler<ChangeTilesToSelectRequest>
{
    protected override void OnItemsSelected(IEnumerable<SelectComponentItem> items, ChangeTilesToSelectRequest request, CancellationToken cancellationToken)
    {
        request.DTO.Task = (BingoTask)items.First().Value!;
    }
}