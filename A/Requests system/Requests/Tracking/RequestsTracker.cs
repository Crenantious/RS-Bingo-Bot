// <copyright file="RequestTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using MediatR;

public class RequestsTracker
{
    private Dictionary<IBaseRequest, RequestTracker> PendingTrackers { get; } = new();
    private Dictionary<IBaseRequest, RequestTracker> ActiveTrackers { get; } = new();

    internal void AddPending(RequestTracker tracker) =>
        PendingTrackers.Add(tracker.Request, tracker);

    internal void ChangePendingToActive(IBaseRequest request)
    {
        ActiveTrackers.Add(request, PendingTrackers[request]);
        PendingTrackers.Remove(request);
    }

    internal RequestTracker GetActive(IBaseRequest request) =>
        ActiveTrackers[request];

    internal void Remove(IBaseRequest request) =>
        ActiveTrackers.Remove(request);

    /// <summary>
    /// Checks the active request handlers with a request that satisfies <paramref name="constraints"/>.
    /// </summary>
    /// <returns>The amount of active handlers that have request satisfying <paramref name="constraints"/>.</returns>
    internal int ActiveCount<TRequest>(Func<TRequest, bool> constraints) =>
        ActiveTrackers.Where(t => t.Key.GetType() == typeof(TRequest))
            .Where(t => constraints((TRequest)t.Key))
            .Count();
}