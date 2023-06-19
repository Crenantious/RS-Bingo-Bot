// <copyright file="DiscordInteractionResponseMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DSharpPlus.Entities;

internal class DiscordInteractionResponseMessage : Message
{
    public bool IsEphemeral { get; }

    public DiscordInteractionResponseMessage(string content, bool isEphemeral) : base(content)
    {
        IsEphemeral = isEphemeral;
    }

    public DiscordInteractionResponseBuilder GetInteractionResponseBuilder()
    {
        throw new NotImplementedException();
    }
}