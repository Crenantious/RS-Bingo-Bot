// <copyright file="SelectComponentHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests.Extensions;

public abstract class SelectComponentHandler<TRequest> : ComponentInteractionHandler<TRequest, SelectComponent>
    where TRequest : ISelectComponentRequest
{
    protected override bool SendKeepAliveMessage => false;

    protected override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        // TODO: JR - maybe put this in a service.
        await Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredMessageUpdate);

        var messageServices = GetRequestService<IDiscordMessageServices>();
        List<SelectComponentOption> options = GetSelectedOptions(request);
        IEnumerable<SelectComponentPage> pages = options.OfType<SelectComponentPage>();

        if (pages.Any())
        {
            await PageSelected(request, pages, cancellationToken);
        }
        else
        {
            await ItemsSelected(request, options, cancellationToken);
        }

        await messageServices.Update(request.GetComponent().Message!);
    }

    private async Task ItemsSelected(TRequest request, List<SelectComponentOption> options, CancellationToken cancellationToken)
    {
        var items = options.Cast<SelectComponentItem>();

        SelectComponentUpdater.ItemsSelected(request.GetComponent(), items);

        OnItemsSelected(items, request, cancellationToken);
        await OnItemSelectedAsync(items, request, cancellationToken);
    }

    private async Task PageSelected(TRequest request, IEnumerable<SelectComponentPage> pages, CancellationToken cancellationToken)
    {
        SelectComponentPage page = pages.ElementAt(0);

        SelectComponentUpdater.PageSlected(request.GetComponent(), page);

        OnPageSelected(page, request, cancellationToken);
        await OnPageSelectedAsync(page, request, cancellationToken);
    }

    protected virtual void OnItemsSelected(IEnumerable<SelectComponentItem> items, TRequest request, CancellationToken cancellationToken) { }
    protected virtual async Task OnItemSelectedAsync(IEnumerable<SelectComponentItem> items, TRequest request, CancellationToken cancellationToken) { }
    protected virtual void OnPageSelected(SelectComponentPage page, TRequest request, CancellationToken cancellationToken) { }
    protected virtual async Task OnPageSelectedAsync(SelectComponentPage page, TRequest request, CancellationToken cancellationToken) { }

    private List<SelectComponentOption> GetSelectedOptions(TRequest request)
    {
        var values = request.GetInteractionArgs().Values;
        List<SelectComponentOption> options = new(values.Length);
        int index;

        for (int i = 0; i < values.Length; i++)
        {
            try
            {
                index = int.Parse(values[i]);
                options.Add(request.GetComponent().Options.ElementAt(index));
            }
            catch
            {
                // Received garbage data
                // TODO: JR - add an appropriate error
                continue;
            }
        }

        return options;
    }
}