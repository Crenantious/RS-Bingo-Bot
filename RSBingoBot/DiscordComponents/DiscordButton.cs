// <copyright file="IDiscordComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DSharpPlus.Entities;
using RSBingoBot.Interfaces;
using RSBingoBot.Requests;

internal class DiscordButton : IDiscordComponent
{
    public DiscordComponent DiscordComponent { get; }
    public IMessage Message { get; set; } = null!;

    public string CustomId => DiscordComponent.CustomId;

    public DiscordButton(DiscordButtonComponent buttonComponent)
    {
        DiscordComponent = buttonComponent;
    }
}