// <copyright file="ScoringServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using FluentResults;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;
using RSBingoBot.Requests;

public class ScoringServices : RequestService, IScoringServices
{
    private readonly LeaderboardMessage leaderboardMessage;

    public ScoringServices(LeaderboardMessage leaderboardMessage)
    {
        this.leaderboardMessage = leaderboardMessage;
    }

    public async Task<Result> SetUpLeaderboardMessage()
    {
        var result = await RunRequest<GetLeaderboardMessageRequest, Message>(new GetLeaderboardMessageRequest());

        if (result.IsSuccess)
        {
            leaderboardMessage.Message = result.Value;
        }

        return new Result()
            .WithReasons(result.Reasons);
    }

    public async Task<Result> UpdateLeaderboard() =>
        await RunRequest(new UpdateLeaderboardRequest(leaderboardMessage.Message));

    public async Task<Result> UpdateTeam(DiscordTeam discordTeam, Team team) =>
        await RunRequest(new UpdateTeamScoreRequest(discordTeam, team));
}