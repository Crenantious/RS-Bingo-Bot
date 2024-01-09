// <copyright file="RequestTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using MediatR;

public class RequestsTracker
{
    public Dictionary<IBaseRequest, RequestTracker> Trackers { get; } = new();
}