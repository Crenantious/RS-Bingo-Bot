// <copyright file="ComponentFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordLibrary.DiscordComponents;
using DSharpPlus;
using DSharpPlus.Entities;

public class ComponentFactory
{
    private readonly ButtonFactory buttonFactory;
    private readonly SelectComponentFactory selectComponentFactory;
    private readonly TextInputFactory textInputFactory;

    public ComponentFactory(ButtonFactory buttonFactory, SelectComponentFactory selectComponentFactory, TextInputFactory textInputFactory)
    {
        this.buttonFactory = buttonFactory;
        this.selectComponentFactory = selectComponentFactory;
        this.textInputFactory = textInputFactory;
    }

    /// <summary>
    /// Creates a <see cref="Component"/> from the <see cref="DiscordComponent"/>.<br/>
    /// Currently only supports <see cref="Button"/>s.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    public Component Create(DiscordComponent discordComponent) =>
        discordComponent.Type switch
        {
            ComponentType.Button => CreateButton((DiscordButtonComponent)discordComponent),
            _ => throw new NotSupportedException()
        };

    private Button CreateButton(DiscordButtonComponent button) =>
        buttonFactory.Create(new ButtonInfo(button.Style, button.Label, button.CustomId, button.Emoji));
}