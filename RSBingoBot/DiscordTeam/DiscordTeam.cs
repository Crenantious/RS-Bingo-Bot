// <copyright file="DiscordTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public class DiscordTeam
{
    public DiscordChannel CategoryChannel { get; set; }
    public DiscordChannel GeneralChannel { get; set; }
    public DiscordChannel BoardChannel { get; set; }
    public DiscordChannel EvidenceChannel { get; set; }
    public DiscordChannel VoiceChannel { get; set; }
    public Message BoardMessage { get; set; }

    public DiscordTeam(DiscordChannel CategoryChannel, DiscordChannel GeneralChannel,
        DiscordChannel BoardChannel, DiscordChannel EvidenceChannel, DiscordChannel VoiceChannel,
        Message BoardMessage)
    {
        this.CategoryChannel = CategoryChannel;
        this.GeneralChannel = GeneralChannel;
        this.BoardChannel = BoardChannel;
        this.EvidenceChannel = EvidenceChannel;
        this.VoiceChannel = VoiceChannel;
        this.BoardMessage = BoardMessage;
    }
}