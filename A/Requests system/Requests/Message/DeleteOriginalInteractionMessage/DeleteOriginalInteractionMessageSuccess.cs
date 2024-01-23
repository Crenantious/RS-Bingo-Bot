// <copyright file="DeleteOriginalInteractionMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class DeleteOriginalInteractionMessageSuccess : MessageSuccess
{
    private const string SuccessMessage = "Deleted interaction original response";

    public DeleteOriginalInteractionMessageSuccess(DiscordInteraction interaction) :
        base(SuccessMessage, interaction.Channel, interaction)
    {

    }
}