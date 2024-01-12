// <copyright file="SendInteractionMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;

internal record SendInteractionMessageRequest(InteractionMessage Message) : IDiscordRequest;