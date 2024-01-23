// <copyright file="DeleteOriginalInteractionMessageError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class DeleteOriginalInteractionMessageError : MessageError
{
    private const string ErrorPrefix = "Unable to delete interaction original response";
    private const string ErrorReason = "The response does not exist (may have been deleted or is ephemeral)";

    public DeleteOriginalInteractionMessageError(DiscordInteraction interaction) :
        base(ErrorPrefix, ErrorReason, null, interaction.Channel, interaction)
    {

    }
}