// <copyright file="IDiscordComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DSharpPlus.Entities;
using RSBingoBot.Interfaces;

public class DiscordButton : IDiscordComponent
{
    public DiscordComponent DiscordComponent { get; }
    public IMessage Message { get; set; } = null!;

    public DiscordButton(DiscordButtonComponent buttonComponent)
    {
        DiscordComponent = buttonComponent;
    }
}