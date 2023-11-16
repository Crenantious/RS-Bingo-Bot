// <copyright file="DiscordButton.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;
using DiscordLibrary.DiscordEntities;

public class DiscordButton : IDiscordComponent
{
    public DiscordComponent DiscordComponent { get; }
    public IMessage? Message { get; set; } = null!;

    public string CustomId => DiscordComponent.CustomId;

    public DiscordButton(DiscordButtonComponent buttonComponent)
    {
        DiscordComponent = buttonComponent;
    }
}