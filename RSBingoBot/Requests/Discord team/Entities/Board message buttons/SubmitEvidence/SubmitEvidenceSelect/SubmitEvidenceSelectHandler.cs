// <copyright file="SubmitEvidenceSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using RSBingo_Framework.Models;

internal class SubmitEvidenceSelectHandler : SelectComponentHandler<SubmitEvidenceSelectRequest>
{
    protected override void OnItemsSelected(IEnumerable<SelectComponentItem> items, SubmitEvidenceSelectRequest request,
        CancellationToken cancellationToken)
    {
        request.DTO.Tiles = items.Select(i => (Tile)i.Value!);
    }
}