// <copyright file="ButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using DSharpPlus.EventArgs;
using RSBingoBot.DiscordComponents;
using RSBingoBot.Requests;

internal abstract class ViewEvidenceSelectionHandler<TRequest> : SelectComponentHandler<TRequest>
    where TRequest : ISelectComponentRequest
{
    protected virtual Task OnItemSelected(ComponentInteractionCreateEventArgs args)
    {
        evidenceTile = (Tile)evidenceSelection.SelectedItems[0].value!;
        Evidence? evidence = evidenceTile.GetEvidence(DataWorker, args.User.Id);
        if (evidence is null)
        {
            // TODO: JR - make this metod use the requests system such that adding an error will post the error message.
            // Currently, this does nothing as this method is called from an event, thus the request system cannot track it.
            // Probably make the SelectComponent have its own request and handler which can be derived from.
            AddError(new EvidenceMissingError(evidenceTile));
        }

        await PostEvidence(evidence);
    }

    protected virtual Task OnPageSelected(ComponentInteractionCreateEventArgs args)
    {
        return Task.CompletedTask;
    }

    protected virtual string OnGetPageName(IEnumerable<SelectComponentOption> options) => 
        $"{options.ElementAt(0).label} - {options.ElementAt(options.Count() - 1).label}";
}