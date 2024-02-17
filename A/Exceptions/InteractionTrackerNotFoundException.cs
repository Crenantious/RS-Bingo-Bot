// <copyright file="InteractionTrackerNotFoundException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

using RSBingo_Common;

internal class InteractionTrackerNotFoundException : Exception
{
    private const string message = "The interaction with id {0} cannot be found.";

    public InteractionTrackerNotFoundException(int id) :
        base(message.FormatConst(id))
    {

    }
}