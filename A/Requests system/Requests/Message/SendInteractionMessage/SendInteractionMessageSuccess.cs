// <copyright file="SendInteractionMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using RSBingo_Common;

internal class SendInteractionMessageSuccess : MessageSuccess
{
    private const string SuccessMessage = "Sent an interaction {0}";

    public SendInteractionMessageSuccess(string responseType, InteractionMessage message) :
        base(SuccessMessage.FormatConst(responseType), message.DiscordMessage, message.Interaction)
    {

    }
}