// <copyright file="AddUserToTeamAddRoleError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;

internal class AddUserToTeamAddRoleError : Error
{
    private const string ErrorMessage = "The team role was unable to be granted to the user '{0}'.";

    public AddUserToTeamAddRoleError(DiscordUser user) : base(ErrorMessage.FormatConst(user.Username))
    {

    }
}