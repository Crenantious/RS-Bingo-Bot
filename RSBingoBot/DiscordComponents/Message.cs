// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DSharpPlus.Entities;

internal class Message
{
    // Max capacity is based on Discord limitations.
    // TODO: JR - Make the max capacity a constant somewhere.
    private List<DiscordActionRowComponent> componentRows = new(5);

    public DiscordMessageBuilder Builder { get; } = new();
    public string Content { get; set; }

    public Message() { }

    public Message(string content) => WithContent(content);

    public Message WithContent(string content)
    {
        Content = content;
        return this;
    }

    public Message AddComponents(params DiscordComponent[] components)
    { 
        componentRows.Add(new(components));  
        return this;
    }

    public Message AddFile()
    {
        throw new NotImplementedException();
    }

    public Message AddImage(Image image)
    {
        throw new NotImplementedException();
    }

    // Probably make the GetBuilder methods extensions.
    public DiscordMessageBuilder GetMessageBuilder() =>
        BuildBaseMessageBuilder(new DiscordMessageBuilder());

    public DiscordWebhookBuilder GetWebhookBuilder() =>
        BuildBaseMessageBuilder(new DiscordWebhookBuilder());

    protected T BuildBaseMessageBuilder<T>(T builder) where T : IDiscordMessageBuilder
    {
        builder.Content = Content;
        componentRows.ForEach(r => builder.AddComponents(r));
        return builder;
    }

    public static Message operator +(Message suffixMessage)
    {
        // Use this to append messages. Combines content, components etc.
        throw new NotImplementedException();
    }
}