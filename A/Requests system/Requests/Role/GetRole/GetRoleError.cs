// <copyright file="SendModalAlreadyRespondedError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using RSBingo_Common;

internal class GetRoleError : Error, IDiscordResponse
{
    private const string ErrorMessage = "The role with id {0} does not exist.";

    public GetRoleError(ulong roleId) : base(ErrorMessage.FormatConst(roleId))
    {

    }
}