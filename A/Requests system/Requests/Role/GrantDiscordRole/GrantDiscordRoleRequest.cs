// <copyright file="GrantDiscordRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record GrantDiscordRoleRequest(DiscordMember Member, DiscordRole Role) : IDiscordRequest;