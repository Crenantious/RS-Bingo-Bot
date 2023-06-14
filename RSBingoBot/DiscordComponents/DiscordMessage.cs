// <copyright file="DiscordMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DSharpPlus.Entities;

internal class DiscordMessage
{
    public string Content { get; }

    public DiscordMessage(string content)
    {
        Content = content;
    }

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

    public static DiscordMessage operator +(DiscordMessage suffixMessage)
    {
        // Use this to append messages. Combines content, components etc.
        throw new NotImplementedException();
    }
}