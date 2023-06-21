// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordEntities;

using DSharpPlus.Entities;
using RSBingoBot.Interfaces;

public class Message : IMessage
{
    public string Content { get; set; } = string.Empty;
    public List<DiscordActionRowComponent> Components { get; set; } = new(5);

    public Message() { }

    // Probably make the GetBuilder methods extensions.
    public DiscordMessageBuilder GetMessageBuilder() =>
        GetBaseMessageBuilder(new DiscordMessageBuilder());

    public DiscordWebhookBuilder GetWebhookBuilder() =>
        GetBaseMessageBuilder(new DiscordWebhookBuilder());

    /// <summary>
    /// Builds the base message builder using <see cref="Content"/> and <see cref="Components"/>.
    /// </summary>
    /// <typeparam name="T">The type of the builder.</typeparam>
    /// <param name="builder">A new instance of the builder.</param>
    protected T GetBaseMessageBuilder<T>(T builder) where T : IDiscordMessageBuilder
    {
        if (string.IsNullOrEmpty(Content) is false) { builder.Content = Content; }
        Components.ForEach(r => builder.AddComponents(r));
        return builder;
    }

    public static Message operator +(Message suffixMessage)
    {
        // Use this to append messages. Combines content, components etc.
        throw new NotImplementedException();
    }
}