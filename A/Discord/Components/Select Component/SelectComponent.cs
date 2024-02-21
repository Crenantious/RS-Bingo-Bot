// <copyright file="SelectComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

// TODO: JR - add a back button to allow the user to go to the previous page.
// This would likely be a button with a SelectComponentBackButton request that takes in the SelectComponent.
public class SelectComponent : Component<DiscordSelectComponent>, ISelectComponentPage, IInteractable
{
    private SelectComponentGetPageName GetPageLabel;
    private IEnumerable<SelectComponentOption> options = Enumerable.Empty<SelectComponentOption>();

    internal List<DiscordSelectComponentOption> DiscordOptions { get; set; } = new();
    internal HashSet<SelectComponentItem> SelectedItemsHashSet { get; set; } = new();
    internal List<SelectComponentItem> selectedItems { get; set; } = new();
    internal List<SelectComponentPage> SelectedPages { get; } = new();

    public override string Name { get; protected set; }
    public string Placeholder { get; } = null!;
    public string Label => GetPageLabel.Get(this);
    public IReadOnlyList<SelectComponentOption> Options { get; private set; } = null!;
    public bool Disabled { get; }
    public int MinOptions { get; }
    public int MaxOptions { get; }

    public SelectComponentPage? SelectedPage { get; internal set; } = null;
    public IReadOnlyList<SelectComponentItem> SelectedItems { get; }

    internal SelectComponent(SelectComponentInfo info, string id = "") : base(id)
    {
        this.Disabled = info.Disabled;
        this.MinOptions = info.MinOptions;
        this.MaxOptions = info.MaxOptions;
        this.Placeholder = info.Label;

        GetPageLabel = SelectComponentGetPageName.CustomMethod(GetLabel);
        Name = $"{info.Label} ({nameof(SelectComponent)})";
        SelectedItems = selectedItems.AsReadOnly();
        SetOptions(info.Options, info.GetPageName);
    }

    /// <param name="getPageLabel">
    /// If the amount of <paramref name="options"/> exceeds the maximum viewable amount, they will be split into pages.
    /// This parameter determines how those pages are named.<br/>
    /// If <see langword="null"/>, <see cref="SelectComponentGetPageName.FirstToLastOptions"/> will be used.
    /// </param>
    public void SetOptions(IEnumerable<SelectComponentOption> options, SelectComponentGetPageName? getPageLabel = null)
    {
        this.options = SelectComponentCommon.TryConvertToPages(options, getPageLabel ?? SelectComponentGetPageName.FirstToLastOptions());
        SetOptions();
    }

    private void SetOptions()
    {
        Options = options.ToList().AsReadOnly();
    }

    // TODO: JR - deal with the label being too long. Maybe after a character count it skips
    // the first x characters to only show the last characters, simulating it being scrolled to the right.
    // If so, prefix with an ellipsis to indicate that's not the whole name.
    private string GetLabel(ISelectComponentPage page) =>
         SelectedPages.Any() ? string.Join(", ", SelectedPages.Select(p => p.Label)) : Placeholder;
}