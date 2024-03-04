// <copyright file="SendInteractionFollowUpSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal class SendInteractionFollowUpSuccess : MessageSuccess
{
    private const string SuccessMessage = "sent an interaction follow up message";

    public SendInteractionFollowUpSuccess(InteractionMessage message) :
        base(SuccessMessage, message.DiscordMessage, message.Interaction)
    {

    }
}