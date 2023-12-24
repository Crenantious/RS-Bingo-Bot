// <copyright file="GetRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal record GetRoleRequest(ulong Id) : IDiscordRequest<DiscordRole>;