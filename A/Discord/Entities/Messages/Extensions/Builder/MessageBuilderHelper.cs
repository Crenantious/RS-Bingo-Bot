// <copyright file="MessageBuilderHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Exceptions;
using DSharpPlus.Entities;
using RSBingo_Common.DataStructures;

internal static class MessageBuilderHelper
{
    public static void AddComponents<T>(DynamicGrid<IComponent> components, T builder) where T : IDiscordMessageBuilder
    {
        foreach (var componentRow in components.GetRows())
        {
            ValidateComponentRow(componentRow);
            builder.AddComponents(GetComponents(componentRow));
        }
    }

    public static void AddFiles<T>(IEnumerable<MessageFile> files, T builder) where T : IDiscordMessageBuilder
    {
        Dictionary<string, Stream> streams = new();
        foreach (MessageFile file in files)
        {
            if (file.HasContent)
            {
                streams.Add(file.DiscordName, file.Open());
            }
        }

        builder.AddFiles(streams);
    }

    private static void ValidateComponentRow(List<IComponent> componentRow)
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
}