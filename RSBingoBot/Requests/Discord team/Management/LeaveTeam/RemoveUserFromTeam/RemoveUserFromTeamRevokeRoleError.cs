// <copyright file="RemoveUserFromTeamError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;

internal class RemoveUserFromTeamError : Error, IDiscordResponse
{
    private const string ErrorMessage = "Was unable to revoke the team's role from the user.";

    public RemoveUserFromTeamError() : base(ErrorMessage)
    {

    }
}