// <copyright file="DeleteTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class DeleteTeamValidator : BingoValidator<DeleteTeamRequest>
{
    public DeleteTeamValidator()
    {
        TeamExists(r => r.DiscordTeam.Id);
    }
}