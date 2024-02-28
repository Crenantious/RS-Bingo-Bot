// <copyright file="IBehaviourServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using FluentResults;

public interface IBehaviourServices : IRequestService
{
    public Task<Result> SendRequestResultsResponse(Message response);
}