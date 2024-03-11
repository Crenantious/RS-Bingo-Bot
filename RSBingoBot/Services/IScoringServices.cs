// <copyright file="IScoringServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using FluentResults;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

public interface IScoringServices : IRequestService
{
    public Task<Result> SetUpLeaderboardMessage();
    public Task<Result> UpdateLeaderboard();
    public Task<Result> UpdateTeam(DiscordTeam discordTeam, Team team);
}