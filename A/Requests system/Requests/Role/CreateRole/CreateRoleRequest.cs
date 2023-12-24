// <copyright file="CreateRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record CreateRoleRequest(string Name) : IDiscordRequest<DiscordRole>;