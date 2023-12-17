// <copyright file="RenameTeamSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using FluentResults;

internal class RenameTeamSuccess : Success, IDiscordResponse
{
    private const string SuccessMessage = "Renamed team from '{0}' to '{1}'.s";

    public RenameTeamSuccess(string oldName, string newName) : base(SuccessMessage.FormatConst(oldName, newName))
    {

    }
}