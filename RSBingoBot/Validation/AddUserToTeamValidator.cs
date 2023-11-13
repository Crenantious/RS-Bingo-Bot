// <copyright file="AddUserToTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class AddUserToTeamValidator : Validator<AddUserToTeamRequest>
{
    public AddUserToTeamValidator()
    {
        UserNotNull(r => r.User);
        UserNotOnATeam(r => r.User);
        TeamExists(r => r.TeamName);
    }
}