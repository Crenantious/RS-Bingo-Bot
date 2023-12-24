﻿// <copyright file="DeleteRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal record DeleteRoleRequest(DiscordRole Role) : IDiscordRequest;