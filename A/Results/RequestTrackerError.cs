// <copyright file="RequestTrackerError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

internal class RequestTrackerError : Error
{
    public RequestTrackerError(Exception e) : base(e.Message)
    {

    }
}