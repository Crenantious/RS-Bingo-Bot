// <copyright file="DeleteRoleSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Common;

internal class DeleteRoleSuccess : Success
{
    private const string SuccessMessage = "A role with the name '{0}' and id {1} has been deleted.";

    public DeleteRoleSuccess(DiscordRole role) : base(SuccessMessage.FormatConst(role.Name, role.Id))
    {

    }
}