// <copyright file="SelectComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public class SelectComponent : IDiscordComponent
{
    internal DiscordSelectComponent discordComponent { get; set; } = null!;
    internal string? placeholder { get; set; } = null;
    internal List<DiscordSelectComponentOption> discordOptions { get; set; } = new();
    internal HashSet<SelectComponentItem> SelectedItemsHashSet { get; set; } = new();
    internal List<SelectComponentItem> selectedItems { get; set; } = new();
    internal List<SelectComponentOption> selectOptions { get; set; } = new();

    public DiscordComponent DiscordComponent => discordComponent;
    public IMessage? Message { get; internal set; }

    public string CustomId { get; } = Guid.NewGuid().ToString();
    public string InitialPlaceholder { get; } = null!;
    public bool Disabled { get; }
    public int MinOptions { get; }
    public int MaxOptions { get; }

    public SelectComponentPage? SelectedPage { get; internal set; } = null;
    public IReadOnlyList<SelectComponentItem> SelectedItems { get; }
    public IReadOnlyList<SelectComponentOption> SelectOptions { get; }

    internal SelectComponent(SelectComponentInfo info)
    {
        this.Disabled = info.Disabled;
        this.selectOptions = info.Options.ToList();
        this.MinOptions = info.MinOptions;
        this.MaxOptions = info.MaxOptions;
        this.placeholder = info.Placeholder;
        this.InitialPlaceholder = placeholder;

        SelectOptions = selectOptions.AsReadOnly();
        SelectedItems = selectedItems.AsReadOnly();
    }
}