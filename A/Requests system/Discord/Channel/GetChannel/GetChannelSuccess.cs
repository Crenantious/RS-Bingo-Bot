// <copyright file="GetChannelSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Common;

internal class GetChannelSuccess : Success
{
    private const string SuccessMessage = "Successfully retrieved channel '{0}' with id {1}.";

    public GetChannelSuccess(DiscordChannel chanel) : base(SuccessMessage.FormatConst(chanel.Name, chanel.Id))
    {

    }
}