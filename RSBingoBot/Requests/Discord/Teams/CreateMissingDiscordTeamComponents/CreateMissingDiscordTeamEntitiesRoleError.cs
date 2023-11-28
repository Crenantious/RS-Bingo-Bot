// <copyright file="CreateMissingDiscordTeamEntitiesRoleError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class CreateMissingDiscordTeamEntitiesRoleError : Error
{
    private const string ErrorMessage = "Unable to create the team's role thus cannot create any channels " +
        "as their permissions must be set for the role.";

    public CreateMissingDiscordTeamEntitiesRoleError() : base(ErrorMessage)
    {

    }
}