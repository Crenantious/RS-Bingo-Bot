// <copyright file="ViewEvidenceSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

internal class ViewEvidenceSelectHandler : SelectComponentHandler<ViewEvidenceSelectRequest>
{
    protected override void OnItemsSelected(IEnumerable<SelectComponentItem> items,
        ViewEvidenceSelectRequest request, CancellationToken cancellationToken)
    {
        // TODO: JR - see if the evidence can be passed to the select component instead of the tile.
        // Not sure if it will be updated after being deleted from another data worker or if it needs
        // to be retrieved like so to check if it still exists.
        Tile tile = (Tile)(items.ElementAt(0).Value!);
        Evidence? evidence = tile.Evidence.FirstOrDefault(e => e.DiscordUserId == request.GetDiscordInteraction().User.Id);
        if (evidence is null)
        {
            AddErrorResponse(new EvidenceMissingError(tile));
            return;
        }

        AddSuccessResponse(new EvidenceFoundSuccess(evidence));
    }
}