// <copyright file="RemoveUserFromTeamRemovedSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.Models;

internal class RemoveUserFromTeamRemovedSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "The user '{0}' has been removed from the team '{1}'.";

    public RemoveUserFromTeamRemovedSuccess(DiscordUser user, Team team) : base(SuccessMessage.FormatConst(user.Username, team.Name))
    {

    }
}