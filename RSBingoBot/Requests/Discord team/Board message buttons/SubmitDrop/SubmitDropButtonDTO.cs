// <copyright file="SubmitDropButtonDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using RSBingo_Framework.Models;

internal class SubmitDropButtonDTO
{
    public Message Message { get; set; }
    public Tile? Tile { get; set; }
    public string? EvidenceUrl { get; set; }

    public SubmitDropButtonDTO(Message message)
    {
        Message = message;
    }
}