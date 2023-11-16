// <copyright file="InteractionSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingoBot.DiscordEntities;
using RSBingoBot.DiscordEntities.Messages;

/// <inheritdoc/>
internal abstract class InteractionSuccess : Success, IInteractionSuccess, IInteractionReason
{
    public InteractionSuccess(Message message) : base(message.Content) =>
        WithMetadata(IInteractionReason.DiscordMessageMetaDataKey, message);

    public InteractionSuccess(string message) : base(message) =>
        WithMetadata(IInteractionReason.DiscordMessageMetaDataKey, new Message().WithContent(message));
}