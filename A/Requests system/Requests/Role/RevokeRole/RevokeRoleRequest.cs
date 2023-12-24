﻿// <copyright file="RevokeRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal record RevokeRoleRequest(DiscordMember Member, DiscordRole Role) : IDiscordRequest;