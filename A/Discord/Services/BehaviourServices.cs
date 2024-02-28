// <copyright file="BehaviourServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using FluentResults;

public class BehaviourServices : RequestService, IBehaviourServices
{
    public async Task<Result> SendRequestResultsResponse(Message response) =>
        await RunRequest(new SendRequestResultResponsesRequest(response));
}