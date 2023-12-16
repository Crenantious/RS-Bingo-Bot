// <copyright file="AddUserToTeamSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingoBot.Discord;

internal class AddUserToTeamSuccess : Success
{
    private const string SuccessMessage = "You have been added to the team '{0}'.";

    public AddUserToTeamSuccess(DiscordTeam discordTeam) : base(SuccessMessage.FormatConst(discordTeam.Team.Name))
    {

    }
}