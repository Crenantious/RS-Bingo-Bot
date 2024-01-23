// <copyright file="SendKeepAliveInteractionMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class SendKeepAliveInteractionMessageSuccess : MessageSuccess
{
    private const string SuccessMessage = "Send a keep alive message in response to an interaction";

    public SendKeepAliveInteractionMessageSuccess(DiscordInteraction interaction) :
        base(SuccessMessage, interaction.Channel, interaction)
    {

    }
}