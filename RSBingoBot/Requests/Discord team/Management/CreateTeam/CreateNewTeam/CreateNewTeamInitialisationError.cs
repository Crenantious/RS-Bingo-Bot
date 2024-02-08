// <copyright file="CreateNewTeamInitialisationError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class CreateNewTeamInitialisationError : Error
{
    private const string ErrorMessage = "The team failed to be created in Discord.";

    public CreateNewTeamInitialisationError() : base(ErrorMessage)
    {

    }
}