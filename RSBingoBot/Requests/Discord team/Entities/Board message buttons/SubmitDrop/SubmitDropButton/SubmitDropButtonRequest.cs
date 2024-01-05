// <copyright file="SubmitDropButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;
using RSBingo_Framework.Records;
using RSBingoBot.Discord;

public record SubmitDropButtonRequest(DiscordTeam DiscordTeam, EvidenceRecord.EvidenceType EvidenceType, int maxSelectOptions) : IButtonRequest
{
    public Button Component { get; set; } = null!;
    public ComponentInteractionCreateEventArgs InteractionArgs { get; set; } = null!;
}