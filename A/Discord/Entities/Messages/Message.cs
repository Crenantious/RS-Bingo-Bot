// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DataStructures;
using DiscordLibrary.DiscordComponents;
using DSharpPlus.Entities;

public class Message : IMessage
{
    /// <summary>
    /// The id for the <see cref="DiscordMessage"/> this is associated with.
    /// Value is only set once the message has been sent, or has been retrieved from an existing <see cref="DiscordMessage"/>.
    /// </summary>
    public ulong Id { get; internal set; }
    public string Content { get; set; } = string.Empty;
    public DynamicGrid<IComponent> Components { get; set; } = new();

    /// <summary>
    /// This can be used to group messages and perform mass operations, e.g. to delete them all together.
    /// </summary>
    public string Tag { get; set; } = string.Empty;

    public Message() { }

    public Message(DiscordMessage message)
    {
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