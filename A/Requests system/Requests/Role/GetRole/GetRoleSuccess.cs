// <copyright file="GetRoleSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Common;

internal class GetRoleSuccess : Success
{
    private const string SuccessMessage = "Retrieved role with the name '{0}' and id {1}.";

    public GetRoleSuccess(DiscordRole role) : base(SuccessMessage.FormatConst(role.Name, role.Id))
    {

    }
}