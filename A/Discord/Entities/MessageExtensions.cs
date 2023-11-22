// <copyright file="MessageExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Exceptions;
using SixLabors.ImageSharp;

public static class MessageExtensions
{
    private const int MaxComponentColumns = 5;
    private const int MaxComponentRows = 5;

    public static T WithContent<T>(this T message, string content)
        where T : Message
    {
        message.Content = content;
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
        ValidateAddComponents(message, components);
        message.Components.AddRow(components);
        return message;
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

    private static void ValidateAddComponents<T>(T message, IComponent[] components)
        where T : Message
    {
        if (message.Components.GetRows().Count >= MaxComponentRows)
        {
            throw new MessageComponentRowsExceededException(MaxComponentRows);
        }

        if (components.Length >= MaxComponentRows)
        {
            throw new MessageComponentColumnsExceededException(MaxComponentColumns);
        }

        if (components.Where(component => component is SelectComponent).Any())
        {
            throw new MessageAddSelectComponentException();
        }
    }
}