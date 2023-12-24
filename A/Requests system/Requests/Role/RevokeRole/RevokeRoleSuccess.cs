// <copyright file="RevokeRoleSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Common;

internal class RevokeRoleSuccess : Success
{
    private const string SuccessMessage = "Revoked role from user. Role name: {0}, role id: {1}, username: {2}, user id: {3}.";

    public RevokeRoleSuccess(DiscordMember member, DiscordRole role) :
        base(SuccessMessage.FormatConst(role.Name, role.Id, member.Username, member.Id))
    {

    }
}