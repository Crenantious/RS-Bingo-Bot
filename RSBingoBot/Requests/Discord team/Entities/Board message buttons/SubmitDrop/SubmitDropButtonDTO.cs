// <copyright file="SubmitDropButtonDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using RSBingo_Framework.Models;
using System.Text;

public class SubmitDropButtonDTO
{
    public string MessageContentPrefix { get; }
    public Message Message { get; set; }
    public IEnumerable<Tile> Tiles { get; set; } = Enumerable.Empty<Tile>();
    public string? EvidenceUrl { get; set; }

    public SubmitDropButtonDTO(Message message)
    {
        MessageContentPrefix = message.Content;
        Message = message;
    }

    public string GetMessageContent() =>
        MessageContentPrefix + (EvidenceUrl is null ? "" : Environment.NewLine + EvidenceUrl);
}