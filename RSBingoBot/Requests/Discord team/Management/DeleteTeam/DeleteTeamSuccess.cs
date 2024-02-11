// <copyright file="DeleteTeamSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingoBot.Discord;

internal class DeleteTeamSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Team '{0}' deleted.";

    public DeleteTeamSuccess(DiscordTeam team) : base(SuccessMessage.FormatConst(team.Name))
    {

    }
}