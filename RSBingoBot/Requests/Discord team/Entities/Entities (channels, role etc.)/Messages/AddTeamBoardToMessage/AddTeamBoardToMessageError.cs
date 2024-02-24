// <copyright file="AddTeamBoardToMessageError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;

internal class AddTeamBoardToMessageError : Error, IDiscordResponse
{
    private const string ErrorMessage = "Failed to create the team's board.";

    public AddTeamBoardToMessageError() : base(ErrorMessage)
    {

    }
}