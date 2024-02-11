// <copyright file="CreateNewTeamSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;

internal class CreateNewTeamSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "The team '{0}' has been created.";

    public CreateNewTeamSuccess(string teamName) : base(SuccessMessage.FormatConst(teamName))
    {

    }
}