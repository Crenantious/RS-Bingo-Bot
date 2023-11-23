// <copyright file="IComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public interface IComponent : IInteractable
{
    /// <summary>
    /// The component to send to Discord.
    /// </summary>
    public DiscordComponent DiscordComponent { get; }

    /// <summary>
    /// The Discord message this component is attached to.
    /// </summary>
    public IMessage? Message { get; }

    public string Name { get; }
}