// <copyright file="RemoveUserFromTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class RemoveUserFromTeamValidator : BingoValidator<RemoveUserFromTeamRequest>
{
    public RemoveUserFromTeamValidator()
    {
        TeamExists(r => r.DiscordTeam.Team);
        UserOnTeam(r => (r.User, r.DiscordTeam.Team.Name));
    }
}