// <copyright file="MessageExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.Exceptions;
using DiscordLibrary.Factories;
using DSharpPlus.Entities;
using RSBingo_Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

public static class MessageExtensions
{
    private const string NullFileName = "Unknown.png";
    private const int MaxComponentColumns = 5;
    private const int MaxComponentRows = 5;

    /// <summary>
    /// Appends <paramref name="content"/> to the current message content.
    /// </summary>
    /// <param name="lineSeparationCount">The amount of lines between the current content and <paramref name="content"/>.</param>
    public static T WithContent<T>(this T message, string content, int lineSeparationCount = 1)
        where T : Message
    {
        if (string.IsNullOrEmpty(message.Content) is false)
        {
            AddContentBlankLines(message, lineSeparationCount);
        }
        message.Content += content;
        return message;
    }

    private static void AddContentBlankLines<T>(T message, int lineSeparationCount) where T : Message
    {
        for (int i = 0; i < lineSeparationCount; i++)
        {
            message.Content += Environment.NewLine;
        }
    }

    /// <summary>
    /// Appends each item in <paramref name="content"/> to the current message content.
    /// </summary>
    /// <param name="lineSeparationCount">The amount of lines between each message in <paramref name="content"/>.
    /// This amount is also appended to the current message content before adding the new <paramref name="content"/>.</param>
    public static T WithContent<T>(this T message, IEnumerable<string> content, int lineSeparationCount = 1)
        where T : Message
    {
        foreach (string item in content)
        {
            WithContent(message, item, lineSeparationCount);
        }
        return message;
    }

    // TODO: JR - certain components, like SelectComponent, can only exist on the row by itself.
    // Set the limits based on component types, probably using IComponentRowLimit for example.
    // So this method would not take SelectComponent but a new method, AddComponent, would.
    /// <summary>
    /// Adds a row of components.
    /// Note that <see cref="SelectComponent"/> cannot be put on a row with other components.
    /// </summary>
    /// <exception cref="MessageComponentRowsExceededException"></exception>
    /// <exception cref="MessageComponentColumnsExceededException"></exception>
    /// <exception cref="MessageAddSelectComponentException"></exception>
    public static T AddComponents<T>(this T message, params IComponent[] components)
        where T : Message
    {
        return AddComponentsCommon(message, components);
    }

    /// <inheritdoc cref="AddComponents{T}(T, IComponent[])"/>
    public static T AddComponents<T>(this T message, IEnumerable<IComponent> components)
        where T : Message
    {
        return AddComponentsCommon(message, components);
    }

    /// <inheritdoc cref="AddComponents{T}(T, IComponent[])"/>
    public static T AddComponents<T>(this T message, IEnumerable<DiscordComponent> components)
        where T : Message
    {
        return AddComponentsCommon(message, components);
    }

    public static T AddFile<T>(this T message, string path, string? name = null)
        where T : Message
    {
        message.FilesInternal.Add((path, name ?? NullFileName));
        return message;
    }

    public static T RemoveAllFiles<T>(this T message)
        where T : Message
    {
        message.FilesInternal.Clear();
        return message;
    }

    public static T AddImage<T>(this T message, Image image, string? name = null)
        where T : Message
    {
        string path = Path.GetTempFileName();
        image.Save(path, new PngEncoder());
        AddFile(message, path, name);
        return message;
    }

    public static void DeleteByTag(this Message message)
    {
        throw new NotImplementedException();
    }

    private static T AddComponentsCommon<T>(this T message, IEnumerable<IComponent> components)
        where T : Message
    {
        ValidateAddComponents(message, components);

        foreach (var component in components)
        {
            component.Message = message;
        }
        message.Components.AddRow(components);
        return message;
    }

    private static T AddComponentsCommon<T>(this T message, IEnumerable<DiscordComponent> components)
        where T : Message
    {
        ComponentFactory componentFactory = (ComponentFactory)General.DI.GetService(typeof(ComponentFactory))!;

        ValidateAddComponents(message, components);
        message.Components.AddRow(components.Select(c => componentFactory.Create(c)));
        return message;
    }

    private static void ValidateAddComponents<T>(T message, IEnumerable<IComponent> components)
        where T : Message
    {
        ValidateAddComponentsCommon(message, components);

        if (components.Count() > 1 && components.Any(c => c is SelectComponent))
        {
            throw new MessageAddSelectComponentException();
        }
    }

    private static void ValidateAddComponents<T>(T message, IEnumerable<DiscordComponent> components)
        where T : Message
    {
        ValidateAddComponentsCommon(message, components);

        if (components.Count() > 1 && components.Any(c => c.IsSelect()))
        {
            throw new MessageAddSelectComponentException();
        }
    }

    private static void ValidateAddComponentsCommon<T>(T message, IEnumerable<object> components)
        where T : Message
    {
        if (message.Components.GetRows().Count >= MaxComponentRows)
        {
            throw new MessageComponentRowsExceededException(MaxComponentRows);
        }

        if (components.Count() >= MaxComponentRows)
        {
            throw new MessageComponentColumnsExceededException(MaxComponentColumns);
        }
    }
}