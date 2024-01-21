// <copyright file="SendInteractionMessageWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using RSBingo_Common;

internal class SendInteractionMessageWarning : MessageSuccess
{
    private const string SuccessMessage = "Sent an interaction {0} but was unable to retrieve it for info";

    public SendInteractionMessageWarning(string responseType, InteractionMessage message) :
        base(SuccessMessage.FormatConst(responseType), message.Interaction.Channel, message.Interaction)
    {

    }
}