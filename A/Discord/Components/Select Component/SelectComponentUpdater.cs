// <copyright file="SelectComponentUpdater.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

internal static class SelectComponentUpdater
{
    /// <summary>
    /// Creates the Discord components.
    /// </summary>
    internal static void Build(SelectComponent selectComponent)
    {
        BuildAllDiscordOptions(selectComponent);
        CreateDiscordSelectComponent(selectComponent);
    }

    internal static void PageSlected(SelectComponent selectComponent, SelectComponentPage page)
    {
        selectComponent.SetOptions(page.Options);
        selectComponent.SelectedPage = page;
        selectComponent.SelectedPages.Add(page);
        selectComponent.selectedItems.Clear();
        selectComponent.SelectedItemsHashSet.Clear();
        Build(selectComponent);
    }

    internal static void ItemsSelected(SelectComponent selectComponent, IEnumerable<SelectComponentItem> items)
    {
        selectComponent.SelectedPage = null;
        selectComponent.selectedItems = items.ToList();
        selectComponent.SelectedItemsHashSet = items.ToHashSet();
        Build(selectComponent);
    }

    private static void BuildAllDiscordOptions(SelectComponent selectComponent)
    {
        selectComponent.DiscordOptions.Clear();

        for (int i = 0; i < selectComponent.Options.Count(); i++)
        {
            SelectComponentOption option = selectComponent.Options.ElementAt(i);
            option.IsDefault = selectComponent.SelectedItemsHashSet.Contains(option);
            option.Build(i.ToString());
            selectComponent.DiscordOptions.Add(option.DiscordOption);
        }
    }

    private static void CreateDiscordSelectComponent(SelectComponent selectComponent)
    {
        int maxOptions = (int)MathF.Min(selectComponent.MaxOptions, selectComponent.DiscordOptions.Count());

        // For maxOptions, the number cannot exceed the amount of discordOptions or there'll be an error
        selectComponent.DiscordComponent = new DiscordSelectComponent(
            selectComponent.CustomId,
            selectComponent.Label,
            selectComponent.DiscordOptions,
            selectComponent.Disabled,
            selectComponent.MinOptions,
            maxOptions);
    }
}