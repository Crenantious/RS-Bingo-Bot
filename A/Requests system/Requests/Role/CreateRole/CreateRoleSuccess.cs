// <copyright file="CreateRoleSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Common;

internal class CreateRoleSuccess : Success
{
    private const string SuccessMessage = "A role with the name '{0}' and id {1} has been created.";

    public CreateRoleSuccess(DiscordRole role) : base(SuccessMessage.FormatConst(role.Name, role.Id))
    {

    }
}