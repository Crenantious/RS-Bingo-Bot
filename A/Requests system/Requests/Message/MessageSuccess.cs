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

    private static string GetMessage(string? action, DiscordMessage? message, DiscordChannel channel, DiscordInteraction? interaction = null)
    {
        StringBuilder sb = new();

        AddBasicInfo(action, message, channel, sb);
        TryAddInteractionInfo(interaction, sb);

        sb.Append(".");
        return sb.ToString();
    }

    private static void AddBasicInfo(string? action, DiscordMessage? message, DiscordChannel channel, StringBuilder sb)
    {
        if (string.IsNullOrWhiteSpace(action))
        {
            action = "null";
        }
        string messageId = message is null ? "null" : message.Id.ToString();
        sb.Append($"Action: {action}. Message id: {messageId}, Channel name: {channel.Name}, Channel id: {channel.Id}");
    }

    private static void TryAddInteractionInfo(DiscordInteraction? interaction, StringBuilder sb)
    {
        if (interaction is not null)
        {
            sb.Append($", interaction id: {interaction.Id}, interaction user: {interaction.User}");
        }
    }
}