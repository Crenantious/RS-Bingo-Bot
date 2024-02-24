// <copyright file="SendKeepAliveInteractionMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class SendKeepAliveInteractionMessageSuccess : MessageSuccess
{
    public SendKeepAliveInteractionMessageSuccess(DiscordInteraction interaction) :
        base(null, interaction.Channel, interaction)
    {

    }
}