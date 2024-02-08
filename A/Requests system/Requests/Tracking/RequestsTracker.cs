// <copyright file="RequestTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.Exceptions;
using MediatR;

public class RequestsTracker
{
    private Dictionary<IBaseRequest, RequestTracker> trackers { get; } = new();

    /// <summary>
    /// Add a tracker that has just entered the pipeline.
    /// </summary>
    internal void Add(RequestTracker tracker) =>
        trackers.Add(tracker.Request, tracker);

    /// <summary>
    /// Get the tracker of an active request.
    /// </summary>
    internal RequestTracker Get(IBaseRequest request)
    {
        if (trackers.ContainsKey(request))
        {
            return trackers[request];
        }

        throw new RequestTrackerNotFoundException(request);
    }

    /// <summary>
    /// Tries to get the tracker of an active request.
    /// </summary>
    internal bool TryGet(IBaseRequest request, out RequestTracker tracker) =>
        trackers.TryGetValue(request, out tracker);

    /// <summary>
    /// Try and remove a request tracker. This should be called when the request has been completed.
    /// </summary>
    internal bool TryRemove(IBaseRequest request)
    {
        if (trackers.ContainsKey(request))
        {
            trackers.Remove(request);
        }
        return false;
    }

    /// <summary>
    /// Checks the active request handlers with a request that satisfies <paramref name="constraints"/>.
    /// </summary>
    /// <returns>The amount of active handlers that have request satisfying <paramref name="constraints"/>.</returns>
    internal int ActiveCount<TRequest>(Func<TRequest, bool> constraints)
        where TRequest : IBaseRequest =>
        trackers.Where(t => t.Key.GetType() == typeof(TRequest))
            .Where(t => constraints((TRequest)t.Key))
            .Count();
}