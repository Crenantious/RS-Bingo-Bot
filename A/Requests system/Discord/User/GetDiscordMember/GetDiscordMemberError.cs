// <copyright file="GetDiscordMemberError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class GetDiscordMemberError : Error
{
    private const string ErrorMessage = "Unable to retrieve the Discord member.";

    public GetDiscordMemberError() : base(ErrorMessage)
    {

    }
}