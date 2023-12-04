// <copyright file="JoinTeamSelectAddedToTeamSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;

internal class JoinTeamSelectAddedToTeamSuccess : Success
{
    private const string SuccessMessage = "The user '{0}' has been added to the team successfully.";

    public JoinTeamSelectAddedToTeamSuccess(DiscordUser user) : base(SuccessMessage.FormatConst(user.Username))
    {

    }
}