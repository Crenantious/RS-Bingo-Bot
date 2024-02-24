// <copyright file="MessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using FluentResults;
using System.Text;

internal class MessageSuccess : Success
{
    public MessageSuccess(string? prefix, DiscordMessage message, DiscordInteraction? interaction = null) :
        base(GetMessage(prefix, message, message.Channel, interaction))
    {

    }

    /// <summary>
    /// Use this if the message was sent but unable to be retrieved to extract it's info.
    /// </summary>
    public MessageSuccess(string? prefix, DiscordChannel channel, DiscordInteraction? interaction = null) :
        base(GetMessage(prefix, null, channel, interaction))
    {

    }

    private static string GetMessage(string? prefix, DiscordMessage? message, DiscordChannel channel, DiscordInteraction? interaction = null)
    {
        string messageId = message is null ? "null" : message.Id.ToString();
        string prefixSuffix = string.IsNullOrWhiteSpace(prefix) ? "" : ". ";
        StringBuilder sb = new($"{prefix}{prefixSuffix}Message id: {messageId}, Channel name: {channel.Name}, Channel id: {channel.Id}");

        if (interaction is not null)
        {
            sb.Append($", interaction id: {interaction.Id}, interaction user: {interaction.User}");
        }

        sb.Append(".");
        return sb.ToString();
    }
}