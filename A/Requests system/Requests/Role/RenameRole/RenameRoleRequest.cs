// <copyright file="RenameRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record RenameRoleRequest(DiscordRole Role, string NewName) : IDiscordRequest;