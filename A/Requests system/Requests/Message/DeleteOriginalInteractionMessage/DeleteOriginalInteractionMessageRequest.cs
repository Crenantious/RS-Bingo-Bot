// <copyright file="DeleteOriginalInteractionMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

namespace DiscordLibrary.Requests;

internal record DeleteOriginalInteractionMessageRequest(DiscordInteraction Interaction) : IDiscordRequest<InteractionMessage>;