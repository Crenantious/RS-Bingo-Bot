// <copyright file="CreateMissingDiscordTeamEntitiesCategoryError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class CreateMissingDiscordTeamEntitiesCategoryError : Error
{
    private const string ErrorMessage = "Unable to create the team's category channel thus cannot continue to create other channels.";

    public CreateMissingDiscordTeamEntitiesCategoryError() : base(ErrorMessage)
    {

    }
}