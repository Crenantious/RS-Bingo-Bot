// <copyright file="SelectComponentPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

public class SelectComponentPage : SelectComponentOption
{
    private SelectComponentGetPageName GetPageLabel;
    private IEnumerable<SelectComponentOption> options = Enumerable.Empty<SelectComponentOption>();

    public override string Label => GetPageLabel.Get(this);

    public IReadOnlyList<SelectComponentOption> Options { get; private set; } = null!;

    public SelectComponentPage(SelectComponentGetPageName getPageLabel, IEnumerable<SelectComponentOption> options,
        SelectComponentGetPageName? getGeneratedPageLabel = null, string? description = null,
        bool isDefault = false, DiscordComponentEmoji? emoji = null) :
        base(description, isDefault, emoji)
    {
        GetPageLabel = getPageLabel;
        SetOptions(options, getGeneratedPageLabel);
    }

    public SelectComponentPage(string label, IEnumerable<SelectComponentOption> options,
        SelectComponentGetPageName? getGeneratedPageLabel = null, string? description = null,
        bool isDefault = false, DiscordComponentEmoji? emoji = null) :
        base(description, isDefault, emoji)
    {
        GetPageLabel = SelectComponentGetPageName.CustomMethod(p => label);
        SetOptions(options, getGeneratedPageLabel);
    }

    public void SetOptions(IEnumerable<SelectComponentOption> options, SelectComponentGetPageName? getGeneratedPageLabel = null)
    {
        this.options = SelectComponentCommon.TryConvertToPages(options, getGeneratedPageLabel ?? SelectComponentGetPageName.FirstToLastOptions());
        SetOptions();
    }

    private void SetOptions()
    {
        Options = options.ToList().AsReadOnly();
    }
}