// <copyright file="RemoveUserFromTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class RemoveUserFromTeamValidator : BingoValidator<RemoveUserFromTeamRequest>
{
    public RemoveUserFromTeamValidator()
    {
        TeamExists(r => r.DiscordTeam.Id);
        UserOnTeam(r => (r.Member, r.DiscordTeam.Id), false);
    }
}