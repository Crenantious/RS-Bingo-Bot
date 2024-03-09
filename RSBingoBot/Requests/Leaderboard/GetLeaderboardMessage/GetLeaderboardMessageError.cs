// <copyright file="GetLeaderboardMessageError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class GetLeaderboardMessageError : Error
{
    private const string ErrorMessage = "Failed to retrieve the leaderboard message with id {id}. " +
        "Make sure one exists and the id is correctly set in the config.";

    public GetLeaderboardMessageError(ulong id) : base(ErrorMessage.FormatConst(id))
    {

    }
}