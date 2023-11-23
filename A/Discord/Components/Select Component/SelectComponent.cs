// <copyright file="SelectComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

public class SelectComponent : Component
{
    internal DiscordSelectComponent DiscordSelectComponent { get; set; } = null!;
    internal string? placeholder { get; set; } = null;
    internal List<DiscordSelectComponentOption> discordOptions { get; set; } = new();
    internal HashSet<SelectComponentItem> SelectedItemsHashSet { get; set; } = new();
    internal List<SelectComponentItem> selectedItems { get; set; } = new();
    internal List<SelectComponentOption> selectOptions { get; set; } = new();
    internal SelectComponentPageName PageName { get; set; }

    public override DiscordComponent DiscordComponent => DiscordSelectComponent;
    public override string Name { get; protected set; }

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

        Name = $"({nameof(SelectComponent)}) {info.Placeholder}";
        PageName = info.pageName ?? SelectComponentPageName.FirstToLastOptions();
        SelectOptions = selectOptions.AsReadOnly();
        SelectedItems = selectedItems.AsReadOnly();
    }
}