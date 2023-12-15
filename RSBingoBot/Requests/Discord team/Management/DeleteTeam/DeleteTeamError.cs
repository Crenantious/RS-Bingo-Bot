// <copyright file="DeleteTeamError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class DeleteTeamError : Error
{
    private const string ErrorMessage = "";

    public DeleteTeamError() : base(ErrorMessage)
    {

    }
}