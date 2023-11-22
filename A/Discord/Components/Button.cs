// <copyright file="Button.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

public class Button : Component
{
    public override DiscordComponent DiscordComponent { get; }

    public Button(ButtonInfo info)
    {
        DiscordComponent = new DiscordButtonComponent(info.Style, Guid.NewGuid().ToString(), info.Label, false, info.Emoji!);
    }
}