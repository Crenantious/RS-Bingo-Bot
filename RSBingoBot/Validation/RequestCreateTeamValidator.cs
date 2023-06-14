// <copyright file="RequestCreateTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Validation;

using FluentResults;
using RSBingoBot.Interfaces;
using RSBingoBot.Requests;

internal class RequestCreateTeamValidator : IValidator<RequestCreateTeam>
{
    public Task<Result> Validate()
    {
        var result = RequestsUtilities.ValidateNewTeamName(teamName, DataWorker);
        AddResponses(result.Responses);
        return result.IsFaulted is false;
    }
}