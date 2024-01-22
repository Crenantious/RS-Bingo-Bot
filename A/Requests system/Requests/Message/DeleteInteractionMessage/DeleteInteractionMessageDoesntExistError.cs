// <copyright file="DeleteInteractionMessageDoesntExistError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal class DeleteInteractionMessageDoesntExistError : MessageError
{
    public DeleteInteractionMessageDoesntExistError(InteractionMessage message) :
        base("Failed to delete interaction message", "the message no longer exists", message.DiscordMessage.Id,
        message.DiscordMessage.Channel, message.Interaction)
    {

    }
}