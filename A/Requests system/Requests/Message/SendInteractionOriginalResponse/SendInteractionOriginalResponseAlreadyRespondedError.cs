// <copyright file="SendInteractionOriginalResponseAlreadyRespondedError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class SendInteractionOriginalResponseAlreadyRespondedError : MessageError
{
    private const string ErrorPrefix = "Failed to send an original interaction response";
    private const string ErrorReason = "The interaction has already been responded to";

    public SendInteractionOriginalResponseAlreadyRespondedError(DiscordInteraction interaction) :
        base(ErrorPrefix, ErrorReason, null, interaction.Channel, interaction)
    {

    }
}