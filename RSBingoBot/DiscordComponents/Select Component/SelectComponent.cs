// <copyright file="SelectComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingoBot.Interfaces;

public class SelectComponent : IDiscordComponent
{
    private DiscordSelectComponent? discordComponent;
    private string? placeholder = null;
    private List<DiscordSelectComponentOption> discordOptions = new();
    private HashSet<SelectComponentItem> SelectedItemsHashSet = new();

    /// <summary>
    /// Gets the discord version of the component. This is null if either <see cref="Build"/> has not been called, or it failed.
    /// </summary>
    public DiscordComponent DiscordComponent => discordComponent;

    /// <inheritdoc/>
    public IMessage? Message { get; set; }
    public SelectComponentPage? SelectedPage { get; private set; } = null;
    public List<SelectComponentItem> SelectedItems { get; private set; } = new();
    public IEnumerable<SelectComponentOption> SelectOptions { get; set; } = Enumerable.Empty<SelectComponentOption>();
    public string CustomId { get; set; } = Guid.NewGuid().ToString();
    public string InitialPlaceholder { get; set; } = null!;
    public bool Disabled { get; set; }
    public int MinOptions { get; set; }
    public int MaxOptions { get; set; }

    public event Func<ComponentInteractionCreateEventArgs, Task>? ItemSelectedCallback;
    public event Func<ComponentInteractionCreateEventArgs, Task>? PageSelectedCallback;
    public event Func<IEnumerable<SelectComponentOption>, string>? GetPageNameCallback;

    public SelectComponent(SelectComponentInfo info)
    {
        this.Disabled = info.Disabled;
        this.SelectOptions = info.Options;
        this.MinOptions = info.MinOptions;
        this.MaxOptions = info.MaxOptions;
        this.placeholder = info.Placeholder;
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
    // TODO: JR - see if this can be made private and called automatically when created.
    public void Build()
    {
        // TODO: JR - add support for building multiple times.
        SelectOptions = SelectComponentCommon.FormatSelectComponentOptions(SelectOptions);
        UpdatePageNames();
        BuildAllDiscordOptions();
        CreateDiscordSelectComponent();
    }

    private void UpdatePageNames()
    {
        if (getPageNameCallback is null) { return; }

        foreach (SelectComponentOption option in SelectOptions)
        {
            if (option is SelectComponentPage)
            {
                option.label = getPageNameCallback(((SelectComponentPage)option).Options);
            }
        }
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
        if (discordComponent == null) { throw new NullReferenceException($"Component must not be null; make sure Build() has been called before this method."); }

        (List<SelectComponentOption> options, OptionSelectionType pageSelected) = GetOptionsFromInteractonArgs(args);

        if (pageSelected is OptionSelectionType.Page)
        {
            await PageSlected(args, (SelectComponentPage)options[0]);
            return;
        }

        await ItemSelected(args, options.Cast<SelectComponentItem>());

        //TODO: JR - dirty the message for it to be rebuilt. This should probably not be done in this class, however.
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
                option = SelectOptions.ElementAt(index);
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

        for (int i = 0; i < SelectOptions.Count(); i++)
        {
            SelectComponentOption option = SelectOptions.ElementAt(i);
            option.isDefault = SelectedItemsHashSet.Contains(option);
            option.Build(i.ToString());
            discordOptions.Add(option.discordOption);
        }
    }

    private void CreateDiscordSelectComponent()
    {
        int maxOptions = (int)MathF.Min(MaxOptions, discordOptions.Count());

        // For maxOptions, the number cannot exceed the amount of discordOptions or there'll be an error
        discordComponent = new DiscordSelectComponent(CustomId, placeholder ?? InitialPlaceholder, discordOptions, Disabled, MinOptions, maxOptions);
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