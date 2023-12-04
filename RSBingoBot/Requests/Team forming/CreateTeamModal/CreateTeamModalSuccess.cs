// <copyright file="CreateTeamModalSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingo_Framework.Models;

internal class CreateTeamModalSuccess : Success
{
    private const string SuccessMessage = "The team '{0}' has been created.";

    public CreateTeamModalSuccess(Team team) : base(SuccessMessage.FormatConst(team))
    {

    }
}