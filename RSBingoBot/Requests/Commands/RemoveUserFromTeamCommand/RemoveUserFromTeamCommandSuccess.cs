// <copyright file="RemoveUserFromTeamCommandSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;

internal class RemoveUserFromTeamCommandSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Removed {0} from team {1}.";

    public RemoveUserFromTeamCommandSuccess(string username, string teamName) : base(SuccessMessage.FormatConst(username, teamName))
    {

    }
}