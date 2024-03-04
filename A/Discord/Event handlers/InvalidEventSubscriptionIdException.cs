// <copyright file="InvalidEventSubscriptionIdException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEventHandlers;

using RSBingo_Common;

internal class InvalidEventSubscriptionIdException : Exception
{
    private const string Message = "{0} is not a valid subscription id.";

    public InvalidEventSubscriptionIdException(int id) : base(Message.FormatConst(id))
    {

    }
}