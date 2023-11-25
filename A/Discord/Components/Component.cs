// <copyright file="Component.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public abstract class Component : IComponent
{
    public abstract DiscordComponent DiscordComponent { get; }
    public IMessage? Message { get; internal set; }

    public string CustomId { get; }

    public abstract string Name { get; protected set; }

    public Component(string id)
    {
        CustomId = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
    }

    internal static Component FromDiscordComponent(DiscordComponent component)
    {
        // TODO: JR - implement
        throw new NotImplementedException();
    }
}