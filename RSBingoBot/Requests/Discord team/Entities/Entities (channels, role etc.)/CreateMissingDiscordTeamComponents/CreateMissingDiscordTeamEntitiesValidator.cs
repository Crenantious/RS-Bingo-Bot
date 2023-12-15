// <copyright file="CreateMissingDiscordTeamEntitiesValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Validation;

internal class CreateMissingDiscordTeamEntitiesValidator : Validator<CreateMissingDiscordTeamEntitiesRequest>
{
    public CreateMissingDiscordTeamEntitiesValidator()
    {
        TeamExists(r => r.DiscordTeam);
    }
}