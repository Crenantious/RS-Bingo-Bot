// <copyright file="SendInteractionOriginalResponseRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus;

public record SendInteractionOriginalResponseRequest(InteractionResponseType ResponseType, InteractionMessage Message) : IDiscordRequest;