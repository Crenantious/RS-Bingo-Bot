// <copyright file="SelectComponentPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

public interface ISelectComponentPage
{
    public string Label { get; }

    public IReadOnlyList<SelectComponentOption> Options { get; }

    public void SetOptions(IEnumerable<SelectComponentOption> options, SelectComponentGetPageName? getPageLabel);
}