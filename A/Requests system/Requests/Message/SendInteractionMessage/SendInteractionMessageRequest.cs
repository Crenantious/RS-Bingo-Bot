// <copyright file="SendInteractionMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

public record SendInteractionMessageRequest(InteractionMessage Message) : IDiscordRequest;