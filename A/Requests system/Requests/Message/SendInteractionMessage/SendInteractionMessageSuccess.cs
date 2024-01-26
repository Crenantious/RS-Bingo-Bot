// <copyright file="SendInteractionMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal class SendInteractionMessageSuccess : MessageSuccess
{
    private const string SuccessMessage = "Sent an interaction message";

    public SendInteractionMessageSuccess(InteractionMessage message) :
        base(SuccessMessage, message.DiscordMessage, message.Interaction)
    {

    }
}