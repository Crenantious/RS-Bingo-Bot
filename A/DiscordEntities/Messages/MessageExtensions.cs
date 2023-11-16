// <copyright file="MessageExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities.Messages;

using RSBingo_Common;
using DiscordLibrary.DiscordComponents;
using SixLabors.ImageSharp;

public static class MessageExtensions
{
    public static TMessage WithContent<TMessage>(this TMessage message, string content)
        where TMessage : IMessage
    {
        message.Content = content;
        return message;
    }

    public static TMessage AddComponents<TMessage>(this TMessage message, params IDiscordComponent[] components)
        where TMessage : IMessage
    {
        components.ForEach(c => c.Message = message);
        // TODO: JR - add method
        message.Components.Add(components);
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