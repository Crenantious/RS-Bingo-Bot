// <copyright file="DeleteTeamCommandValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class DeleteTeamCommandValidator : BingoValidator<DeleteTeamCommandRequest>
{
    public DeleteTeamCommandValidator()
    {
        TeamExists(r => r.TeamName);
    }
}