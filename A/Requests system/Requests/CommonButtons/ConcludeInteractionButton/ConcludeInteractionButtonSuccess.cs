// <copyright file="ConcludeInteractionButtonSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using RSBingo_Common;

internal class ConcludeInteractionButtonSuccess : Success
{
    private const string SuccessMessage = "Concluded interaction with tracker id {0}.";

    public ConcludeInteractionButtonSuccess(IInteractionTracker tracker) : base(SuccessMessage.FormatConst(tracker.Id))
    {

    }
}