// <copyright file="ConcludeInteractionButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

/// <param name="User">Only allows interactions from <paramref name="User"/>, unless it's null</param>
public record ConcludeInteractionButtonRequest(IInteractionTracker Tracker, IEnumerable<Message>? MessagesToDelete = null,
    DiscordUser? User = null) :
    IButtonRequest;