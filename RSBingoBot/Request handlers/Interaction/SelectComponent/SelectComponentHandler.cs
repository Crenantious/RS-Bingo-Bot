// <copyright file="ButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers;

using DSharpPlus.EventArgs;
using RSBingoBot.DiscordComponents;
using RSBingoBot.InteractionHandlers;
using RSBingoBot.Requests;
using System.Threading;

// TODO: JR - make each method run through RequestHandler. Handle by mapping a keys to the methods.
// The keys will be passed to the ISelectComponentRequest.
// This way each method is run in the try-catch, validated and logged just like other request handlers.
internal abstract class SelectComponentHandler<TRequest> : InteractionHandler<TRequest>
    where TRequest : ISelectComponentRequest
{
    protected override Task Process(TRequest request, CancellationToken cancellationToken)
    {
        request.SelectComponent.ItemSelectedCallback += OnItemSelected;
        request.SelectComponent.PageSelectedCallback += OnPageSelected;
        request.SelectComponent.GetPageNameCallback += OnGetPageName;
        return base.Process(request, cancellationToken);
    }

    protected abstract Task OnItemSelected(ComponentInteractionCreateEventArgs args);

    protected virtual Task OnPageSelected(ComponentInteractionCreateEventArgs args)
    {
        return Task.CompletedTask;
    }

    protected abstract string OnGetPageName(IEnumerable<SelectComponentOption> options);
}