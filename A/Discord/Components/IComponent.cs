// <copyright file="IComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public interface IComponent
{
    /// <summary>
    /// The component to send to Discord.
    /// </summary>
    public DiscordComponent DiscordComponent { get; }

    /// <summary>
    /// The Discord message this component is attached to.
    /// </summary>
    public IMessage? Message { get; }

    /// <summary>
    /// Used as an identifier for Discord. Set a unique value if the component
    /// needs to be retrieved from a message.
    /// </summary>
    public string CustomId { get; }

    /// <summary>
    /// For logging purposes.
    /// </summary>
    public string Name { get; }
}