// <copyright file="ILeaderboardServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using FluentResults;

public interface ILeaderboardServices : IRequestService
{
    public Task<Result> GetMessage();
    public Task<Result> UpdateMessage();
}