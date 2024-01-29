// <copyright file="DeleteTeamCommandSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class DeleteTeamCommandSuccess : Success
{
    private const string SuccessMessage = "";

    public DeleteTeamCommandSuccess() : base(SuccessMessage)
    {

    }
}