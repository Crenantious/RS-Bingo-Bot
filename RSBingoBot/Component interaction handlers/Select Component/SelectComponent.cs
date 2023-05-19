// <copyright file="SelectComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers.Select_Component;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

public class SelectComponent
{
    /// <summary>
    /// Gets the discord version of the component. This is null if either <see cref="Build"/> has not been called, or it failed.
    /// </summary>
    public DiscordSelectComponent? DiscordComponent { get; private set; }
    public SelectComponentPage? SelectedPage { get; private set; } = null;
    public List<SelectComponentItem> SelectedItems { get; private set; } = new();
    public List<SelectComponentOption> SelectOptions { get; set; } = new();
    public string CustomId { get; set; } = null!;
    public string InitialPlaceholder { get; set; } = null!;
    public bool Disabled { get; set; }
    public int MinOptions { get; set; }
    public int MaxOptions { get; set; }

    private string? placeholder = null;
    private Func<ComponentInteractionCreateEventArgs, Task>? itemSelectedCallback;
    private Func<ComponentInteractionCreateEventArgs, Task>? pageSelectedCallback;
    private List<DiscordSelectComponentOption> discordOptions = new();
    private HashSet<SelectComponentItem> SelectedItemsHashSet = new();

    public SelectComponent(string customId, string placeholder,
        Func<ComponentInteractionCreateEventArgs, Task>? itemSelectedCallback = null,
        Func<ComponentInteractionCreateEventArgs, Task>? pageSelectedCallback = null,
        bool disabled = false, int minOptions = 1, int maxOptions = 1)
    {
        this.CustomId = customId;
        this.itemSelectedCallback = itemSelectedCallback;
        this.pageSelectedCallback = pageSelectedCallback;
        this.Disabled = disabled;
        this.MinOptions = minOptions;
        this.MaxOptions = maxOptions;
        this.placeholder = placeholder;
        this.InitialPlaceholder = placeholder;
    }

    private enum OptionSelectionType
    {
        Page,
        Item
    }

    /// <summary>
    /// Builds the structure of the pages and items, creates the corresponding <see cref="DiscordSelectComponentOption"/>s
    /// and the <see cref="DiscordSelectComponent"/>.
    /// This should be called before <see cref="OnInteraction"/>.
    /// </summary>
    public void Build()
    {
        // TODO: JR - add support for building multiple times.
        SelectOptions = SelectComponentCommon.FormatSelectComponentOptions(SelectOptions);
        BuildAllDiscordOptions();
        CreateDiscordSelectComponent();
    }

    /// <summary>
    /// This should be subscribed to the <see cref="ComponentInteractionDEH"/> so it is called when
    /// the <see cref="DiscordSelectComponent"/> is interacted with.
    /// </summary>
    /// <param name="client">The client the interaction occurred on.</param>
    /// <param name="args">The arguments for the interaction</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task OnInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        if (DiscordComponent == null) { throw new NullReferenceException($"Component must not be null; make sure Build() has been called before this method."); }

        (List<SelectComponentOption> options, OptionSelectionType pageSelected) = GetOptionsFromInteractonArgs(args);

        if (pageSelected is OptionSelectionType.Page)
        {
            await PageSlected(args, (SelectComponentPage)options[0]);
            return;
        }

        await ItemSelected(args, options.Cast<SelectComponentItem>());
    }

    private (List<SelectComponentOption>, OptionSelectionType) GetOptionsFromInteractonArgs(ComponentInteractionCreateEventArgs args)
    {
        List<SelectComponentOption> options = new(args.Values.Length);
        SelectComponentOption option;
        int index;

        for (int i = 0; i < args.Values.Length; i++)
        {
            try
            {
                index = int.Parse(args.Values[i]);
                option = SelectOptions[index];
            }
            catch
            {
                // Received garbage data
                continue;
            }

            if (option.GetType().IsAssignableFrom(typeof(SelectComponentPage)))
            {
                options = new() { option };

                // If a page is selected with an item or another page, the first page will be loaded and the other
                // selections will be ignored.
                return (options, OptionSelectionType.Page);
            }

            options.Add(option);
        }

        return (options, OptionSelectionType.Item);
    }

    private void BuildAllDiscordOptions()
    {
        discordOptions.Clear();

        for (int i = 0; i < SelectOptions.Count; i++)
        {
            SelectComponentOption option = SelectOptions[i];
            option.isDefault = SelectedItemsHashSet.Contains(option);
            option.Build(i.ToString());
            discordOptions.Add(option.discordOption);
        }
    }

    private void CreateDiscordSelectComponent()
    {
        int maxOptions = (int)MathF.Min(MaxOptions, discordOptions.Count());

        // For maxOptions, the number cannot exceed the amount of discordOptions or there'll be an error
        DiscordComponent = new DiscordSelectComponent(CustomId, placeholder ?? InitialPlaceholder, discordOptions, Disabled, MinOptions, maxOptions);
    }

    private async Task PageSlected(ComponentInteractionCreateEventArgs args, SelectComponentPage page)
    {
        SelectedPage = page;
        SelectOptions = page.Options;
        UpdatePlaceholder(page.label);
        SelectedItems.Clear();
        SelectedItemsHashSet.Clear();
        Build();

        if (pageSelectedCallback != null)
        {
            await pageSelectedCallback.Invoke(args);
        }
    }

    private async Task ItemSelected(ComponentInteractionCreateEventArgs args, IEnumerable<SelectComponentItem> items)
    {
        SelectedPage = null;
        SelectedItems = new(items);
        SelectedItemsHashSet = new(items);
        BuildAllDiscordOptions();
        CreateDiscordSelectComponent();

        if (itemSelectedCallback != null)
        {
            await itemSelectedCallback(args);
        }
    }

    private void UpdatePlaceholder(string pageLabel)
        => placeholder = IsFirstPage() ? pageLabel : $"{placeholder}, {pageLabel}";

    private bool IsFirstPage() => placeholder == InitialPlaceholder;
}