// <copyright file="DeleteTeamCommandRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingoBot.Discord;

internal record DeleteTeamCommandRequest(DiscordTeam Team) : ICommandRequest;