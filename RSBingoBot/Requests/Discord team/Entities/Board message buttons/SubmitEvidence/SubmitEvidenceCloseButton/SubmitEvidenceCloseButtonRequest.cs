// <copyright file="SubmitEvidenceCloseButtonRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;

internal record SubmitEvidenceCloseButtonRequest(IInteractionTracker Tracker, IEnumerable<Message> MessagesToDelete,
    DiscordUser User, int? MessageCreatedDEHSubscriptionId) : ConcludeInteractionButtonRequest(Tracker, MessagesToDelete, User);