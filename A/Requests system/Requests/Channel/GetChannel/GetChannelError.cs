// <copyright file="GetChannelError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingo_Common;

internal class GetChannelError : Error
{
    private const string ErrorMessage = "Failed to retrieve channel with id '{0}'.";

    public GetChannelError(ulong id) : base(ErrorMessage.FormatConst(id))
    {

    }
}