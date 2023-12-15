// <copyright file="CreateChannelSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Common;

internal class CreateChannelSuccess : Success
{
    private const string SuccessMessage = "A channel with the name '{0}' and id {1} has been created.";

    public CreateChannelSuccess(DiscordChannel channel) : base(SuccessMessage.FormatConst(channel.Name, channel.Id))
    {

    }
}