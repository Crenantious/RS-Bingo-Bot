// <copyright file="RemoveUserFromTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class RemoveUserFromTeamValidator : Validator<RemoveUserFromTeamRequest>
{
    public RemoveUserFromTeamValidator()
    {
        TeamExists(r => r.Team);
        UserOnTeam(r => (r.User, r.Team));
    }
}