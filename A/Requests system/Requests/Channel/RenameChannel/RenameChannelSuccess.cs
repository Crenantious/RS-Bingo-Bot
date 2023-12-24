// <copyright file="RenameChannelSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using RSBingo_Common;

internal class RenameChannelSuccess : Success
{
    private const string SuccessMessage = "Renamed channel. Old name: {0}, new name: {1}.";

    public RenameChannelSuccess(string oldName, string newName) : base(SuccessMessage.FormatConst(oldName, newName))
    {

    }
}