// <copyright file="SelectComponentPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

public class SelectComponentPage : SelectComponentOption, ISelectComponentPage
{
    private SelectComponentGetPageName GetPageLabel;
    private IEnumerable<SelectComponentOption> options = Enumerable.Empty<SelectComponentOption>();

    public override string Label => GetPageLabel.Get(this);

    public IReadOnlyList<SelectComponentOption> Options { get; private set; } = null!;

    public SelectComponentPage(SelectComponentGetPageName getPageLabel, string? description = null,
        bool isDefault = false, DiscordComponentEmoji? emoji = null) :
        base(description, isDefault, emoji)
    {
        GetPageLabel = getPageLabel;
        SetOptions();
    }

    public SelectComponentPage(string label, string? description = null,
        bool isDefault = false, DiscordComponentEmoji? emoji = null) :
        base(description, isDefault, emoji)
    {
        GetPageLabel = SelectComponentGetPageName.CustomMethod(p => label);
        SetOptions();
    }

    public void SetOptions(IEnumerable<SelectComponentOption> options, SelectComponentGetPageName? getPageLabel = null)
    {
        this.options = SelectComponentCommon.TryConvertToPages(options, getPageLabel ?? SelectComponentGetPageName.FirstToLastOptions());
        SetOptions();
    }

    private void SetOptions()
    {
        Options = options.ToList().AsReadOnly();
    }
}