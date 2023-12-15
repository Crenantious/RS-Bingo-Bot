// <copyright file="GetDiscordMemberSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Common;

internal class GetDiscordMemberSuccess : Success
{
    private const string SuccessMessage = "Retrieved the Discord member with username '{0}' and id {1}.";

    public GetDiscordMemberSuccess(DiscordMember member) : base(SuccessMessage.FormatConst(member.Username, member.Id))
    {

    }
}