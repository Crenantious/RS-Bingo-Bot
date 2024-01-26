// <copyright file="SendInteractionFollowUpError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

internal class SendInteractionFollowUpError : MessageError
{
    private const string ErrorPrefix = "Failed to send an interaction followup message";
    private const string ErrorReason = nameof(BadRequestException);

    public SendInteractionFollowUpError(DiscordInteraction interaction) :
        base(ErrorPrefix, ErrorReason, null, interaction.Channel, interaction)
    {

    }
}