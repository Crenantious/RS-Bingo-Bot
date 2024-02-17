﻿// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DataStructures;
using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Exceptions;
using DSharpPlus.Entities;

public class Message : IMessage
{
    /// <summary>
    /// The <see cref="DSharpPlus.Entities.DiscordMessage"/> this is associated with.
    /// Value is only set once the message has been sent, or has been retrieved from an existing <see cref="DSharpPlus.Entities.DiscordMessage"/>.
    /// </summary>
    public DiscordMessage DiscordMessage { get; internal set; }

    public string Content { get; set; } = string.Empty;
    public DynamicGrid<IComponent> Components { get; set; } = new();

    /// <summary>
    /// This can be used to group messages and perform mass operations, e.g. to delete them all together.
    /// </summary>
    public string Tag { get; set; } = string.Empty;

    public Message() { }

    public Message(DiscordMessage message)
    {
        DiscordMessage = message;
        this.WithContent(message.Content);

        foreach (var row in message.Components)
        {
            this.AddComponents(row.Components);
        }
    }

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
        builder.Content = Content;

        foreach (var componentRow in Components.GetRows())
        {
            ValidateComponentRowForBuilder(componentRow);
            builder.AddComponents(GetComponents(componentRow));
        }

        return builder;
    }

    private static void ValidateComponentRowForBuilder(List<IComponent> componentRow)
    {
        if (componentRow.Count == 0)
        {
            throw new MessageComponentRowEmptyException();
        }

        if (componentRow.ElementAt(0) is SelectComponent)
        {
            var selectComponent = (SelectComponent)componentRow.ElementAt(0);
            if (selectComponent.SelectOptions.Any() is false)
            {
                throw new SelectComponentNoOptionsException();
            }
        }
    }

    private static IEnumerable<DiscordComponent> GetComponents(List<IComponent> row) =>
        row.Select(c => c.GetDiscordComponent());

    public static Message operator +(Message prefix, Message suffix)
    {
        // TODO: JR - implement.
        // Use this to append messages. Combines content, components etc.

        prefix.WithContent(suffix.Content);
        return prefix;
    }
}