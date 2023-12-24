// <copyright file="CreateTeamRoleRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;

internal record CreateTeamRoleRequest(RSBingoBot.Discord.DiscordTeam DiscordTeam) : IDiscordRequest<DiscordRole>;