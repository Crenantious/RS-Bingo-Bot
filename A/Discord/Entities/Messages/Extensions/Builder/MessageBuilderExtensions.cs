// <copyright file="MessageExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Exceptions;
using DSharpPlus.Entities;

public static class MessageBuilderExtensions
{
    public static DiscordMessageBuilder GetMessageBuilder(this Message message) =>
        GetBaseMessageBuilder(message, new DiscordMessageBuilder());

    /// <summary>
    /// Builds the base message builder using <see cref="Content"/> and <see cref="Components"/>.
    /// </summary>
    /// <typeparam name="T">The type of the builder.</typeparam>
    /// <param name="builder">A new instance of the builder.</param>
    public static T GetBaseMessageBuilder<T>(this Message message, T builder) where T : IDiscordMessageBuilder
    {
        builder.Content = message.Content;

        AddBuilderComponents(message, builder);
        AddBuilderFiles(message, builder);

        return builder;
    }

    private static void AddBuilderComponents<T>(Message message, T builder) where T : IDiscordMessageBuilder
    {
        foreach (var componentRow in message.Components.GetRows())
        {
            ValidateComponentRowForBuilder(componentRow);
            builder.AddComponents(GetComponents(componentRow));
        }
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

    private static void AddBuilderFiles<T>(Message message, T builder) where T : IDiscordMessageBuilder
    {
        Dictionary<string, Stream> streams = new();
        foreach (MessageFile file in message.FilesInternal)
        {
            if (file.HasContent)
            {
                streams.Add(file.DiscordName, file.Open());
            }
        }

        builder.AddFiles(streams);
    }
}