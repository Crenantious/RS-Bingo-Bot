// <copyright file="SelectComponentHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;

public abstract class SelectComponentHandler<TRequest> : InteractionHandler<TRequest>
    where TRequest : ISelectComponentRequest
{
    protected override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        List<SelectComponentOption> options = GetSelectedOptions(MetaData.Get<SelectComponent>(),
            MetaData.Get<ComponentInteractionCreateEventArgs>());
        IEnumerable<SelectComponentPage> pages = options.OfType<SelectComponentPage>();

        if (pages.Any())
        {
            await OnPageSelected(pages.ElementAt(0), request, cancellationToken);
            return;
        }

        await OnItemSelected(options.Cast<SelectComponentItem>(), request, cancellationToken);
    }

    protected abstract Task OnPageSelected(SelectComponentPage page, TRequest request, CancellationToken cancellationToken);

    protected abstract Task OnItemSelected(IEnumerable<SelectComponentItem> page, TRequest request, CancellationToken cancellationToken);

    private List<SelectComponentOption> GetSelectedOptions(SelectComponent selectComponent, ComponentInteractionCreateEventArgs args)
    {
        List<SelectComponentOption> options = new(args.Values.Length);
        int index;

        for (int i = 0; i < args.Values.Length; i++)
        {
            try
            {
                index = int.Parse(args.Values[i]);
                options.Add(selectComponent.selectOptions.ElementAt(index));
            }
            catch
            {
                // Received garbage data
                continue;
            }
        }

        return options;
    }
}