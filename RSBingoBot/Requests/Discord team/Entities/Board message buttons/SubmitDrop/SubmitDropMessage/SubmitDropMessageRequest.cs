// <copyright file="SubmitDropMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;

public record SubmitDropMessageRequest(SubmitDropButtonDTO DTO) : IMessageCreatedRequest
{
    public MessageCreateEventArgs MessageArgs { get; set; } = null!;
    public Message Message { get; set; } = null!;
}