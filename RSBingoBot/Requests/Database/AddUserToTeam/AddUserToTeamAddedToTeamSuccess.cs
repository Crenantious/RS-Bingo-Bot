// <copyright file="AddUserToTeamAddedToTeamSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;

internal class AddUserToTeamAddedToTeamSuccess : Success
{
    private const string SuccessMessage = "The user '{0}' has been added to the team successfully.";

    public AddUserToTeamAddedToTeamSuccess(DiscordUser user) : base(SuccessMessage.FormatConst(user.Username))
    {

    }
}