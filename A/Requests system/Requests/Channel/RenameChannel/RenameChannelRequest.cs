﻿// <copyright file="RenameChannelRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public record RenameChannelRequest(DiscordChannel Channel, string NewName) : IDiscordRequest;