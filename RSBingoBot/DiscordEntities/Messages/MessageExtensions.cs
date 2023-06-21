// <copyright file="MessageExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordEntities.Messages;

using DSharpPlus.Entities;
using RSBingoBot.Interfaces;

internal static class MessageExtensions
{
    public static TMessage WithContent<TMessage>(this TMessage message, string content)
        where TMessage : IMessage
    {
        message.Content = content;
        return message;
    }

    public static TMessage AddComponents<TMessage>(this TMessage message, params DiscordComponent[] components)
    where TMessage : IMessage
    {
        message.Components.Add(new(components));
        return message;
    }

    public static TMessage AddFile<TMessage>(this TMessage message, string path)
        where TMessage : IMessage
    {
        throw new NotImplementedException();
    }

    public static TMessage AddImage<TMessage>(this TMessage message, Image image)
        where TMessage : IMessage
    {
        throw new NotImplementedException();
    }
}