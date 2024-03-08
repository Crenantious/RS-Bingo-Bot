// <copyright file="PostLeaderboardCommandRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;

internal record PostLeaderboardCommandRequest(DiscordChannel Channel) : ICommandRequest;