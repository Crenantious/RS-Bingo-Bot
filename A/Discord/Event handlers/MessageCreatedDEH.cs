// <copyright file="MessageCreatedDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEventHandlers;

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

public class MessageCreatedDEH : DiscordEventHandlerBase<MessageCreateEventArgs>
{
    public record Constraints(DiscordChannel? channel = null, DiscordUser? author = null,
        int? numberOfAttachments = null, string? attatchmentFileExtension = null);

    public MessageCreatedDEH() =>
        Client.MessageCreated += OnEvent;
}