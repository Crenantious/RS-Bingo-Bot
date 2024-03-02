// <copyright file="IMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordComponents;
using DSharpPlus.Entities;
using RSBingo_Common.DataStructures;

// TODO: JR - change a lot of references of Message to IMessage and ensure IMessage has all of Message's methods.
public interface IMessage
{
    /// <summary>
    /// The <see cref="DSharpPlus.Entities.DiscordMessage"/> this is associated with.
    /// Value is only set once the message has been sent, or has been retrieved from an existing <see cref="DSharpPlus.Entities.DiscordMessage"/>.
    /// </summary>
    public DiscordMessage DiscordMessage { get; }
    public string Content { get; set; }
    public DynamicGrid<IComponent> Components { get; set; }

    public DiscordMessageBuilder GetMessageBuilder();
    public void OnMessageSent(DiscordMessage discordMessage);
}