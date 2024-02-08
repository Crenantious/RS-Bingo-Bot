// <copyright file="Button.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;

public class Button : Component<DiscordButtonComponent>, IInteractable
{
    public override string Name { get; protected set; }

    public Button(ButtonInfo info) : base(info.Id)
    {
        Name = $"({nameof(Button)}) {info.Label}";
        DiscordComponent = new DiscordButtonComponent(info.Style, CustomId, info.Label, false, info.Emoji!);
    }
}