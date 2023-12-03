// <copyright file="SetDiscordTeamExistingEntitiesValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class SetDiscordTeamExistingEntitiesValidator : Validator<SetDiscordTeamExistingEntitiesRequest>
{
    public SetDiscordTeamExistingEntitiesValidator()
    {
        DiscordTeamNotNull(r => r.DiscordTeam);
    }
}