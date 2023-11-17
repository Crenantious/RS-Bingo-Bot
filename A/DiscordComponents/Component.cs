// <copyright file="Component.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public abstract class Component : IDiscordComponent
{
    public abstract DiscordComponent DiscordComponent { get; }
    public IMessage? Message { get; internal set; }

    public string CustomId { get; } = Guid.NewGuid().ToString();

    internal Component()
    {

    }
}