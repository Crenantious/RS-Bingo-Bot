// <copyright file="ConcludeInteractionButtonSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using RSBingo_Common;

internal class ConcludeInteractionButtonSuccess : Success
{
    private const string SuccessMessage = "Concluded interaction with request handler with id {0}.";

    public ConcludeInteractionButtonSuccess(IRequestHandler handler) : base(SuccessMessage.FormatConst(handler.Id))
    {

    }
}