// <copyright file="DeleteTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class DeleteTeamValidator : BingoValidator<DeleteTeamRequest>
{
    public DeleteTeamValidator()
    {
        TeamExists(r => r.TeamName);
    }
}