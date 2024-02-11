// <copyright file="DeleteTeamCommandRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

internal record DeleteTeamCommandRequest(string TeamName) : ICommandRequest;