// <copyright file="GetChannelError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal class SendModalError : MessageError
{
    private const string ErrorPrefix = "Failed to send modal.";
    private const string ErrorReason = "Interaction already responded to.";

    public SendModalError(Modal modal) : base(ErrorPrefix, ErrorReason, null, modal.Interaction.Channel, modal.Interaction)
    {

    }
}