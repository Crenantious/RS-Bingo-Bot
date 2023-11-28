﻿// <copyright file="CreateTeamRoleValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Validation;

internal class CreateTeamRoleValidator : Validator<CreateTeamRoleRequest>
{
    public CreateTeamRoleValidator()
    {
        DiscordTeamNotNull(r => r.DiscordTeam);
        TeamExists(r => r.DiscordTeam.Team);
    }
}