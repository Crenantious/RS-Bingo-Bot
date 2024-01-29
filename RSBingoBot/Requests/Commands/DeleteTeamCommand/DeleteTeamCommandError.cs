// <copyright file="DeleteTeamCommandError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class DeleteTeamCommandError : Error
{
    private const string ErrorMessage = "";

    public DeleteTeamCommandError() : base(ErrorMessage)
    {

    }
}