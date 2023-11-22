// <copyright file="DiscordInteractionResponseMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using DSharpPlus.Entities;

// TODO: JR - determine if this is needed.
internal class DiscordInteractionResponseMessage : Message
{
    public bool IsEphemeral { get; }

    public DiscordInteractionResponseMessage(bool isEphemeral)
    {
        IsEphemeral = isEphemeral;
    }

    public DiscordInteractionResponseBuilder GetInteractionResponseBuilder()
    {
        throw new NotImplementedException();
    }
}