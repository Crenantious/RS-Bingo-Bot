// <copyright file="AddUserToTeamSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingo_Framework.Models;

internal class AddUserToTeamSuccess : Success
{
    private const string SuccessMessage = "You have been added to the team '{0}'.";

    public AddUserToTeamSuccess(Team team) : base(SuccessMessage.FormatConst(team.Name))
    {

    }
}