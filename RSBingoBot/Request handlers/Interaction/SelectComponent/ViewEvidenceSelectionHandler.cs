// <copyright file="ButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.RequestHandlers;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

internal class ViewEvidenceSelectionHandler : SelectComponentHandler<ViewEvidenceSelectionRequest>
{
    protected override string OnGetPageName(IEnumerable<SelectComponentOption> options) =>
        $"{options.ElementAt(0).label} - {options.ElementAt(options.Count() - 1).label}";

    protected override void OnItemSelected(IEnumerable<SelectComponentItem> items,
        ViewEvidenceSelectionRequest request, CancellationToken cancellationToken)
    {
        Tile tile = (Tile)(items.ElementAt(0).Value!);
        Evidence? evidence = tile.Evidence.FirstOrDefault(e => e.DiscordUserId == InteractionArgs.User.Id);
        if (evidence is null)
        {
            AddError(new EvidenceMissingError(tile));
            return;
        }

        AddSuccess(new EvidenceFoundSuccess(evidence));
    }
}