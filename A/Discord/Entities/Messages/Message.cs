// <copyright file="Message.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DataStructures;
using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Exceptions;
using DSharpPlus.Entities;

public class Message : IMessage
{
    internal List<MessageFile> FilesInternal { get; set; } = new();

    public IReadOnlyList<MessageFile> Files => FilesInternal.AsReadOnly();

    public DiscordMessage DiscordMessage { get; private set; }

    public string Content { get; set; } = string.Empty;
    public DynamicGrid<IComponent> Components { get; set; } = new();
    public DiscordChannel Channel { get; set; }

    /// <summary>
    /// This can be used to group messages and perform mass operations, e.g. to delete them all together.
    /// </summary>
    public string Tag { get; set; } = string.Empty;

    public Message(DiscordChannel channel)
    {
        Channel = channel;
    }

    public Message(DiscordMessage message)
    {
        DiscordMessage = message;
        Channel = message.Channel;

        if (message.Content is not null)
        {
            this.WithContent(message.Content);
        }

        if (message.Components is not null)
        {
            foreach (var row in message.Components)
            {
                this.AddComponents(row.Components);
            }
        }
    }

    // Probably make the GetBuilder methods extensions.
    public DiscordMessageBuilder GetMessageBuilder() =>
        GetBaseMessageBuilder(new DiscordMessageBuilder());

    public void OnMessageSent(DiscordMessage discordMessage)
    {
        DiscordMessage = discordMessage;
        FilesInternal.ForEach(f => f.Close());
    }

    /// <summary>
    /// Builds the base message builder using <see cref="Content"/> and <see cref="Components"/>.
    /// </summary>
    /// <typeparam name="T">The type of the builder.</typeparam>
    /// <param name="builder">A new instance of the builder.</param>
    protected T GetBaseMessageBuilder<T>(T builder) where T : IDiscordMessageBuilder
    {
        builder.Content = Content;

        AddBuilderComponents(builder);
        AddBuilderFiles(builder);

        return builder;
    }

    private void AddBuilderComponents<T>(T builder) where T : IDiscordMessageBuilder
    {
        foreach (var componentRow in Components.GetRows())
        {
            ValidateComponentRowForBuilder(componentRow);
            builder.AddComponents(GetComponents(componentRow));
        }
    }

    private void AddBuilderFiles<T>(T builder) where T : IDiscordMessageBuilder
    {
        Dictionary<string, Stream> streams = new();
        FilesInternal.ForEach(f => streams.Add(f.Name, f.Open()));
        builder.AddFiles(streams);
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
            selectComponent.Validate();
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