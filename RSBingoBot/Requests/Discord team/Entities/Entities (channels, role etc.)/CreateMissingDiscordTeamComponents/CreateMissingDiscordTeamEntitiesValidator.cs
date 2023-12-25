// <copyright file="CreateMissingDiscordTeamEntitiesValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class CreateMissingDiscordTeamEntitiesValidator : BingoValidator<CreateMissingDiscordTeamEntitiesRequest>
{
    public CreateMissingDiscordTeamEntitiesValidator()
    {
        TeamExists(r => r.DiscordTeam.Team);
    }
}