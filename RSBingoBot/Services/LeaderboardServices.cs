// <copyright file="LeaderboardServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using FluentResults;
using RSBingoBot.Discord;
using RSBingoBot.Requests;

public class LeaderboardServices : RequestService, ILeaderboardServices
{
    private readonly LeaderboardMessage leaderboardMessage;

    public LeaderboardServices(LeaderboardMessage leaderboardMessage)
    {
        this.leaderboardMessage = leaderboardMessage;
    }

    public async Task<Result> GetMessage()
    {
        var result = await RunRequest<GetLeaderboardMessageRequest, Message>(new GetLeaderboardMessageRequest());

        if (result.IsSuccess)
        {
            leaderboardMessage.Message = result.Value;
        }

        return new Result()
            .WithReasons(result.Reasons);
    }

    public async Task<Result> UpdateMessage() =>
        await RunRequest(new UpdateLeaderboardRequest(leaderboardMessage.Message));
}