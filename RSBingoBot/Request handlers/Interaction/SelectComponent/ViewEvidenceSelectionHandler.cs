// <copyright file="ViewEvidenceSelectionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.RequestHandlers;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

internal class ViewEvidenceSelectionHandler : SelectComponentHandler<ViewEvidenceSelectionRequest>
{
    protected override void OnItemsSelected(IEnumerable<SelectComponentItem> items,
        ViewEvidenceSelectionRequest request, CancellationToken cancellationToken)
    {
        Tile tile = (Tile)(items.ElementAt(0).Value!);
        Evidence? evidence = tile.Evidence.FirstOrDefault(e => e.DiscordUserId == InteractionArgs.Interaction.User.Id);
        if (evidence is null)
        {
            AddError(new EvidenceMissingError(tile));
            return;
        }

        AddSuccess(new EvidenceFoundSuccess(evidence));
    }
}