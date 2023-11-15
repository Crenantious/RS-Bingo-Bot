// <copyright file="InteractionError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingoBot.DiscordEntities;

/// <summary>
/// Make sure to use <see cref="DiscordMessage"/> instead of <see cref="InteractionError.Message"/>.
/// </summary>
internal class InteractionError : Error
{
    public Message DiscordMessage { get; set; }

    public InteractionError(Message message) =>
        this.DiscordMessage = message;
}