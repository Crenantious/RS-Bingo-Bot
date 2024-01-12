// <copyright file="MessageError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using FluentResults;
using System.Text;

internal class MessageError : Error
{
    /// <param name="prefix">E.g. Failed to send a message.</param>
    public MessageError(string prefix, string reason, ulong? messageId = null, DiscordChannel? channel = null,
        DiscordInteraction? discordInteraction = null) :
        base(GetMessage(prefix, reason, messageId, channel, discordInteraction))
    {

    }

    private static string GetMessage(string prefix, string reason, ulong? messageId = null, DiscordChannel? channel = null,
        DiscordInteraction? interaction = null)
    {
        StringBuilder message = new($"{prefix}. Reason: {reason}");

        if (messageId is not null)
        {
            message.Append($", id: {messageId}");
        }

        if (channel is not null)
        {
            message.Append($", channel name: {channel.Name}, channel id: {channel.Id}");
        }

        if (interaction is not null)
        {
            message.Append($", interaction id: {interaction.Id}, interaction user: {interaction.User}");
        }

        message.Append(".");
        return message.ToString();
    }
}