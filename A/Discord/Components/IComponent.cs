// <copyright file="IComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public interface IComponent
{
    // TODO: JR - make this internal set only. Probably inherit IWriteableComponent and have each
    // concrete component have their own interface (e.g. IButton) which only inherits IComponent.
    // Factories will return IButton which doesn't have access to IWriteableComponent, but to set
    // the message, the internal processor will have to cast the IComponent to IWriteableComponent.
    // Ask Josh if there is a better way.
    /// <summary>
    /// The Discord message this component is attached to.
    /// </summary>
    public Message? Message { get; set; }

    /// <summary>
    /// Used as an identifier for Discord. Set a unique value if the component
    /// needs to be retrieved from a message.
    /// </summary>
    public string CustomId { get; }

    /// <summary>
    /// For logging purposes.
    /// </summary>
    public string Name { get; }

    public DiscordComponent GetDiscordComponent();
}