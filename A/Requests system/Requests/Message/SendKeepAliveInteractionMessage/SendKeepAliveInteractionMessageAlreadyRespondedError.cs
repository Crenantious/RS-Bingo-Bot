// <copyright file="SendKeepAliveInteractionMessageAlreadyRespondedError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class SendKeepAliveInteractionMessageAlreadyRespondedError : MessageError
{
    private const string ErrorMessage = "Failed to send a keep alive message in response to an interaction";
    private const string ErrorReason = "The interaction has already been responded to";

    public SendKeepAliveInteractionMessageAlreadyRespondedError(DiscordInteraction interaction) :
        base(ErrorMessage, ErrorReason, null, interaction.Channel, interaction)
    {

    }
}