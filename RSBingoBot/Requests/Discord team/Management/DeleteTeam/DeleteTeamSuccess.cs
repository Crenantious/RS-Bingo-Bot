// <copyright file="DeleteTeamSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class DeleteTeamSuccess : Success
{
    private const string SuccessMessage = "";

    public DeleteTeamSuccess() : base(SuccessMessage)
    {

    }
}