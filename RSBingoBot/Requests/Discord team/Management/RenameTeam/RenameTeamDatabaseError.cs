// <copyright file="RenameTeamDatabaseError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;

internal class RenameTeamDatabaseError : Error, IDiscordResponse
{
    private const string ErrorMessage = "Failed to rename the team in the database.";

    public RenameTeamDatabaseError() : base(ErrorMessage)
    {

    }
}