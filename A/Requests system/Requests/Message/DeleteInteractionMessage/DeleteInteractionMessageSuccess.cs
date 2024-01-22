// <copyright file="DeleteInteractionMessageSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal class DeleteInteractionMessageSuccess : MessageSuccess
{
    private const string SuccessMessage = "Deleted an interaction message";

    public DeleteInteractionMessageSuccess(InteractionMessage message) : base(SuccessMessage, message.DiscordMessage, message.Interaction)
    {

    }
}