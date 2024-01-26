// <copyright file="SendInteractionFollowUpRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

public record SendInteractionFollowUpRequest(InteractionMessage Message) : IDiscordRequest;