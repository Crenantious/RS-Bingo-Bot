// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordEntities;

using DSharpPlus.Entities;
using RSBingoBot.DiscordComponents;
using RSBingoBot.DTO;
using RSBingoBot.Interfaces;
using System.Collections.Generic;

public class Message : IMessage
{
    public string Content { get; set; } = string.Empty;
    public DynamicGrid<IDiscordComponent> Components { get; set; } = new(5);

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

        Components.GetRows().ForEach(r =>
            builder.AddComponents(
                r.Select(c => c.DiscordComponent)
                    .ToArray()));

        return builder;
    }

    public static Message operator +(Message suffixMessage)
    {
        // TODO: JR - implement.
        // Use this to append messages. Combines content, components etc.
        throw new NotImplementedException();
    }
}