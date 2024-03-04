// <copyright file="SendInteractionOriginalResponseSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal class SendInteractionOriginalResponseSuccess : MessageSuccess
{
    private const string SuccessMessage = "sent an original interaction response";

    public SendInteractionOriginalResponseSuccess(InteractionMessage message) :
        base(SuccessMessage, message.DiscordMessage, message.Interaction)
    {

    }
}