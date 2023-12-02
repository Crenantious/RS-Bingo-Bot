// <copyright file="SelectComponentHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;

public abstract class SelectComponentHandler<TRequest> : ComponentInteractionHandler<TRequest, SelectComponent>
    where TRequest : ISelectComponentRequest
{
    protected override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        List<SelectComponentOption> options = GetSelectedOptions(request);
        IEnumerable<SelectComponentPage> pages = options.OfType<SelectComponentPage>();

        if (pages.Any())
        {
            OnPageSelected(pages.ElementAt(0), request, cancellationToken);
            await OnPageSelectedAsync(pages.ElementAt(0), request, cancellationToken);
            return;
        }

        OnItemsSelected(options.Cast<SelectComponentItem>(), request, cancellationToken);
        await OnItemSelectedAsync(options.Cast<SelectComponentItem>(), request, cancellationToken);
    }

    protected virtual void OnItemsSelected(IEnumerable<SelectComponentItem> items, TRequest request, CancellationToken cancellationToken) { }
    protected virtual async Task OnItemSelectedAsync(IEnumerable<SelectComponentItem> items, TRequest request, CancellationToken cancellationToken) { }
    protected virtual void OnPageSelected(SelectComponentPage page, TRequest request, CancellationToken cancellationToken) { }
    protected virtual async Task OnPageSelectedAsync(SelectComponentPage page, TRequest request, CancellationToken cancellationToken) { }

    private List<SelectComponentOption> GetSelectedOptions(TRequest request)
    {
        List<SelectComponentOption> options = new(InteractionArgs.Values.Length);
        int index;

        for (int i = 0; i < InteractionArgs.Values.Length; i++)
        {
            try
            {
                index = int.Parse(InteractionArgs.Values[i]);
                options.Add(request.Component.selectOptions.ElementAt(index));
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