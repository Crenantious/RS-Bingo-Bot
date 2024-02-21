// <copyright file="SelectComponentInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

// TODO: JR - rename
public record SelectComponentInfo(string Label, IEnumerable<SelectComponentOption> Options, SelectComponentGetPageName? GetPageName = null,
    bool Disabled = false, int MinOptions = 1, int MaxOptions = 1);