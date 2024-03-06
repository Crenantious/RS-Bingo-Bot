// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordComponents;
using DSharpPlus.Entities;
using RSBingo_Common.DataStructures;

public class Message
{
    internal List<MessageFile> FilesInternal { get; set; } = new();

    public IReadOnlyList<MessageFile> Files => FilesInternal.AsReadOnly();

    public DiscordMessage DiscordMessage { get; internal set; } = null!;

    public string Content { get; set; } = string.Empty;
    public DynamicGrid<IComponent> Components { get; set; } = new();
    public DiscordChannel Channel { get; set; }

    /// <summary>
    /// This can be used to group messages and perform mass operations, e.g. to delete them all together.
    /// </summary>
    public string Tag { get; set; } = string.Empty;

    internal Message(DiscordChannel channel)
    {
        Channel = channel;
    }

    public DiscordMessageBuilder GetMessageBuilder() =>
        GetBaseMessageBuilder(new DiscordMessageBuilder());

    /// <summary>
    /// Builds the base message builder using <see cref="Content"/> and <see cref="Components"/>.
    /// </summary>
    /// <typeparam name="T">The type of the builder.</typeparam>
    /// <param name="builder">A new instance of the builder.</param>
    public T GetBaseMessageBuilder<T>(T builder) where T : IDiscordMessageBuilder
    {
        builder.Content = Content;

        MessageBuilderHelper.AddComponents(Components, builder);
        MessageBuilderHelper.AddFiles(FilesInternal, builder);

        return builder;
    }

    public static Message operator +(Message prefix, Message suffix)
    {
        // TODO: JR - implement.
        // Use this to append messages. Combines content, components etc.

        prefix.WithContent(suffix.Content);
        return prefix;
    }
}