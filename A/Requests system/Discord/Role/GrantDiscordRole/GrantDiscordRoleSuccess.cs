// <copyright file="GrantDiscordRoleSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Common;

internal class GrantDiscordRoleSuccess : Success
{
    private const string SuccessMessage = "Granted the role with the name '{0}' and id {1} to the user '{2}' with id {3}.";

    public GrantDiscordRoleSuccess(DiscordRole role, DiscordMember member) :
        base(SuccessMessage.FormatConst(role.Name, role.Id, member.Username, member.Id))
    {

    }
}