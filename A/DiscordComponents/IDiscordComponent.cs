// <copyright file="IDiscordComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DSharpPlus.Entities;
using DiscordLibrary.DiscordEntities;

public interface IDiscordComponent : IInteractable
{
    /// <summary>
    /// The component to send to Discord.
    /// </summary>
    public DiscordComponent DiscordComponent { get; }

    /// <summary>
    /// The Discord message this component is attached to.
    /// </summary>
    public IMessage? Message { get; }
}