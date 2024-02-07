// <copyright file="RenameTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class RenameTeamValidator : BingoValidator<RenameTeamRequest>
{
    public RenameTeamValidator()
    {
        TeamExists(r => r.DiscordTeam.Id);
        NewTeamName(r => r.NewName);
    }
}