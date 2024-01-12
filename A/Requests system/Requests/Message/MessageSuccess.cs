// <copyright file="MessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using FluentResults;
using System.Text;

internal class MessageSuccess : Success
{
    public MessageSuccess(string prefix, DiscordMessage message, DiscordInteraction? interaction = null) :
        base(GetMessage(prefix, message, interaction))
    {

    }

    private static string GetMessage(string prefix, DiscordMessage message, DiscordInteraction? interaction = null)
    {
        StringBuilder sb = new($"{prefix}. Id: {message.Id}, Channel name: {message.Channel.Name}, Channel id: {message.Channel.Id}");

        if (interaction is not null)
        {
            sb.Append($", interaction id: {interaction.Id}, interaction user: {interaction.User}");
        }

        sb.Append(".");
        return sb.ToString();
    }
}