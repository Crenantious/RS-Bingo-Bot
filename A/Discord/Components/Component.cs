// <copyright file="Component.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public abstract class Component<TDiscordComponent> : IComponent
    where TDiscordComponent : DiscordComponent
{
    public TDiscordComponent DiscordComponent { get; internal set; }

    public IMessage? Message { get; set; }

    public string CustomId { get; }

    public abstract string Name { get; protected set; }

    public Component(string id)
    {
        CustomId = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
    }

    public DiscordComponent GetDiscordComponent() =>
        DiscordComponent;

    public void SetMessage(IMessage message) =>
        Message = message;
}