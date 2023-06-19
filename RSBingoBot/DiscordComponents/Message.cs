// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DSharpPlus.Entities;

internal class Message
{
    public DiscordMessageBuilder Builder { get; }

    public Message() { }

    public Message(string content)
    {
        Builder = new();
        Builder.WithContent(content);
    }

    public Message(DiscordMessageBuilder builder) => Builder = builder;

    public void AddComponents()
    {
        throw new NotImplementedException();
    }

    public void AddFile()
    {
        throw new NotImplementedException();
    }

    // Probably make the GetBuilder methods extensions.
    public DiscordMessageBuilder GetMessageBuilder()
    {
        throw new NotImplementedException(); 
    }

    public DiscordWebhookBuilder GetWebhookBuilder()
    {
        throw new NotImplementedException();
    }

    public static Message operator +(Message suffixMessage)
    {
        // Use this to append messages. Combines content, components etc.
        throw new NotImplementedException();
    }
}