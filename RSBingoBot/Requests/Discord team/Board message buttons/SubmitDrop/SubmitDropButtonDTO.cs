// <copyright file="SubmitDropButtonDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using RSBingo_Framework.Models;

internal class SubmitDropButtonDTO
{
    public string MessageContentPrefix { get; }
    public Message Message { get; set; }
    public Tile? Tile { get; set; }
    public string? EvidenceUrl { get; set; }

    public SubmitDropButtonDTO(string messageContentPrefix, Message message)
    {
        MessageContentPrefix = messageContentPrefix;
        Message = message;
    }

    public string GetMessageContent() =>
        MessageContentPrefix + (EvidenceUrl is null ? "" : Environment.NewLine + EvidenceUrl);
}