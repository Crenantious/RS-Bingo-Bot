// <copyright file="RevokeRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record RevokeRoleRequest(DiscordMember Member, DiscordRole Role) : IDiscordRequest;