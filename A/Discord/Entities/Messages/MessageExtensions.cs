// <copyright file="MessageExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Exceptions;
using DiscordLibrary.Factories;
using DSharpPlus.Entities;
using RSBingo_Common;
using SixLabors.ImageSharp;

public static class MessageExtensions
{
    private const int MaxComponentColumns = 5;
    private const int MaxComponentRows = 5;

    /// <summary>
    /// Appends <paramref name="content"/> to the current message content.
    /// </summary>
    /// <param name="lineSeparationCount">The amount of lines between the current content and <paramref name="content"/>.</param>
    public static T WithContent<T>(this T message, string content, int lineSeparationCount = 1)
        where T : Message
    {
        for (int i = 0; i < lineSeparationCount; i++)
        {
            message.Content += Environment.NewLine;
        }
        message.Content += content;
        return message;
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

    public static T AddFile<T>(this T message)
        where T : Message
    {
        throw new NotImplementedException();
    }

    public static T AddImage<T>(this T message, Image image)
        where T : Message
    {
        throw new NotImplementedException();
    }

    public static void Send(this Message message, DiscordChannel channel)
    {
        var service = (IDiscordMessageServices)General.DI.GetService(typeof(IDiscordMessageServices))!;
        service.Send(message, channel);
    }

    public static void Delete(this Message message)
    {
        throw new NotImplementedException();
    }

    public static void DeleteByTag(this Message message)
    {
        throw new NotImplementedException();
    }

    private static T AddComponentsCommon<T>(this T message, IEnumerable<IComponent> components)
        where T : Message
    {
        ValidateAddComponents(message, components);
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

        if (components.Any(c => c is SelectComponent))
        {
            throw new MessageAddSelectComponentException();
        }
    }

    private static void ValidateAddComponents<T>(T message, IEnumerable<DiscordComponent> components)
        where T : Message
    {
        ValidateAddComponentsCommon(message, components);

        if (components.Any(c => c.IsSelect()))
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