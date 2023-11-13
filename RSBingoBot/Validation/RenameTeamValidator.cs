// <copyright file="RenameTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class RenameTeamValidator : Validator<RenameTeamRequest>
{
    public RenameTeamValidator()
    {
        TeamExists(r => r.TeamName);
        NewTeamName(r => r.NewTeamName);
    }
}