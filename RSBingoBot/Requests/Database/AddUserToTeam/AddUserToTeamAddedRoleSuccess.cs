// <copyright file="AddUserToTeamAddedRoleSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;

internal class AddUserToTeamAddedRoleSuccess : Success
{
    private const string SuccessMessage = "The team role has been granted to the user '{0}' successfully.";

    public AddUserToTeamAddedRoleSuccess(DiscordUser user) : base(SuccessMessage.FormatConst(user.Username))
    {

    }
}