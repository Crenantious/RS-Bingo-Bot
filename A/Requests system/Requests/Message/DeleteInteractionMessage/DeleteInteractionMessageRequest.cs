// <copyright file="DeleteInteractionMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

public record DeleteInteractionMessageRequest(InteractionMessage Message) : IDiscordRequest;