// <copyright file="SelectComponentUpdater.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

internal static class SelectComponentUpdater
{
    // TODO: JR - is this needed? Can't we just use the SelectComponentPage.Label?
    internal static void UpdatePageNames(SelectComponent selectComponent, Func<SelectComponentPage, string> getPageName)
    {
        foreach (SelectComponentOption option in selectComponent.selectOptions)
        {
            if (option is SelectComponentPage)
            {
                option.label = getPageName(((SelectComponentPage)option).Options);
            }
        }
    }

    /// <summary>
    /// Builds the structure of the pages and items, creates the corresponding <see cref="DiscordSelectComponentOption"/>s
    /// and the <see cref="DiscordSelectComponent"/>.
    /// This should be called before <see cref="OnInteraction"/>.
    /// </summary>
    internal static void Build(SelectComponent selectComponent)
    {
        // TODO: JR - add support for building multiple times.
        selectComponent.selectOptions = SelectComponentCommon.FormatSelectComponentOptions(selectComponent.selectOptions);
        // Is this needed?
        UpdatePageNames(selectComponent);
        BuildAllDiscordOptions(selectComponent);
        CreateDiscordSelectComponent(selectComponent);
    }

    internal static void PageSlected(SelectComponent selectComponent, SelectComponentPage page)
    {
        selectComponent.SelectedPage = page;
        selectComponent.selectOptions = page.Options;
        selectComponent.selectedItems.Clear();
        selectComponent.SelectedItemsHashSet.Clear();
        UpdatePlaceholder(selectComponent, page.label);
        Build(selectComponent);
    }

    internal static void ItemsSelected(SelectComponent selectComponent, IEnumerable<SelectComponentItem> items)
    {
        selectComponent.SelectedPage = null;
        selectComponent.selectedItems = items.ToList();
        selectComponent.SelectedItemsHashSet = items.ToHashSet();
        BuildAllDiscordOptions(selectComponent);
        CreateDiscordSelectComponent(selectComponent);
    }

    private static void BuildAllDiscordOptions(SelectComponent selectComponent)
    {
        selectComponent.discordOptions.Clear();

        for (int i = 0; i < selectComponent.selectOptions.Count(); i++)
        {
            SelectComponentOption option = selectComponent.selectOptions.ElementAt(i);
            option.isDefault = selectComponent.SelectedItemsHashSet.Contains(option);
            option.Build(i.ToString());
            selectComponent.discordOptions.Add(option.discordOption);
        }
    }

    private static void CreateDiscordSelectComponent(SelectComponent selectComponent)
    {
        int maxOptions = (int)MathF.Min(selectComponent.MaxOptions, selectComponent.discordOptions.Count());

        // For maxOptions, the number cannot exceed the amount of discordOptions or there'll be an error
        selectComponent.discordComponent = new DiscordSelectComponent(
            selectComponent.CustomId,
            selectComponent.placeholder ?? selectComponent.InitialPlaceholder,
            selectComponent.discordOptions,
            selectComponent.Disabled,
            selectComponent.MinOptions,
            maxOptions);
    }

    private static void UpdatePlaceholder(SelectComponent selectComponent, string pageLabel)
        => selectComponent.placeholder =
            IsFirstPage(selectComponent) ?
            pageLabel :
            $"{selectComponent.placeholder}, {pageLabel}";

    private static bool IsFirstPage(SelectComponent selectComponent) =>
       selectComponent.placeholder == selectComponent.InitialPlaceholder;
}