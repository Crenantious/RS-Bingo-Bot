// <copyright file="IDiscordComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DSharpPlus.Entities;
using RSBingoBot.Interfaces;

internal interface IDiscordComponent : IInteractable
{
    /// <summary>
    /// The component to send to Discord.
    /// </summary>
    public DiscordComponent DiscordComponent { get; }

    /// <summary>
    /// The Discord message this component is attached to.
    /// </summary>
    public IMessage? Message { get; set; }
}