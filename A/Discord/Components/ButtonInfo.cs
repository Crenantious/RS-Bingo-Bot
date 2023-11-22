// <copyright file="ButtonInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus;
using DSharpPlus.Entities;

public record ButtonInfo(ButtonStyle Style, string Label, DiscordComponentEmoji? Emoji = null);