// <copyright file="GetRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record GetRoleRequest(ulong Id) : IDiscordRequest<DiscordRole>;