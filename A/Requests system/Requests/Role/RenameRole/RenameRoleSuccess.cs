// <copyright file="RenameRoleSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using RSBingo_Common;

internal class RenameRoleSuccess : Success
{
    private const string SuccessMessage = "Renamed role. Oldname: {0}, new name {1}.";

    public RenameRoleSuccess(string oldName, string newName) : base(SuccessMessage.FormatConst(oldName, newName))
    {

    }
}