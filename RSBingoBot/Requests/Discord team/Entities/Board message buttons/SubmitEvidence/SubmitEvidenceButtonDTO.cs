﻿// <copyright file="SubmitEvidenceButtonDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using RSBingo_Framework.Models;

public class SubmitEvidenceButtonDTO
{
    public string MessageContentPrefix { get; }

    /// <summary>
    /// The response message that the user interacts with to submit evidence.
    /// </summary>
    public Message Message { get; set; }
    public IEnumerable<Tile> Tiles { get; set; } = Enumerable.Empty<Tile>();
    public string? EvidenceUrl { get; set; }

    public SubmitEvidenceButtonDTO(Message message)
    {
        MessageContentPrefix = message.Content;
        Message = message;
    }
}