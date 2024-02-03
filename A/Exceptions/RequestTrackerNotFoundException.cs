// <copyright file="RequestTrackerNotFoundException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Exceptions;

using MediatR;
using RSBingo_Common;

internal class RequestTrackerNotFoundException : Exception
{
    private const string message = "The request tracker for request of type {0} cannot be found.";

    public RequestTrackerNotFoundException(IBaseRequest request) :
        base(message.FormatConst(request))
    {

    }
}