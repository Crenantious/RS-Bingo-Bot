// <copyright file="SelectComponentHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests.Extensions;

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
        var values = request.GetInteractionArgs().Values;
        List<SelectComponentOption> options = new(values.Length);
        int index;

        for (int i = 0; i < values.Length; i++)
        {
            try
            {
                index = int.Parse(values[i]);
                options.Add(request.GetComponent().selectOptions.ElementAt(index));
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