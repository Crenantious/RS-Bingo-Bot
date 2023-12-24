// <copyright file="GetMessageRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

internal record GetMessageRequest(ulong Id, DiscordChannel Channel) : IDiscordRequest<Message>;