// <copyright file="InteractionMessageExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

internal static class InteractionMessageExtensions
{
    public static T AsEphemeral<T>(this T message, bool status)
        where T : InteractionMessage
    {
        message.IsEphemeral = status;
        return message;
    }
}