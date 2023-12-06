// <copyright file="SubmitDropSubmitButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;
using RSBingo_Framework.Records;

internal record SubmitDropSubmitButtonRequest(SubmitDropButtonDTO DTO, EvidenceRecord.EvidenceType EvidenceType) : IButtonRequest
{
    public Button Component { get; set; } = null!;
    public ComponentInteractionCreateEventArgs InteractionArgs { get; set; } = null!;
}