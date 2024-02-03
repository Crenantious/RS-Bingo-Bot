// <copyright file="RequestTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.Exceptions;
using FluentResults;
using MediatR;
using System.Text;

public class RequestsTracker
{
    private Dictionary<IBaseRequest, RequestTracker> PendingTrackers { get; } = new();
    private Dictionary<IBaseRequest, RequestTracker> ActiveTrackers { get; } = new();

    /// <summary>
    /// Add a tracker that has just entered the pipeline. This should be moved to Active when it's handler is entered.
    /// </summary>
    internal void AddPending(RequestTracker tracker) =>
        PendingTrackers.Add(tracker.Request, tracker);

    /// <summary>
    /// Set a request to be active. This occurs when its handler is entered and begins processing.
    /// </summary>
    internal TResult ChangePendingToActive<TResult>(IBaseRequest request)
        where TResult : ResultBase<TResult>, new()
    {
        StringBuilder error = new();

        if (PendingTrackers.ContainsKey(request) is false)
        {
            error.AppendLine("The request is not pending thus can't be made active.");
        }

        if (ActiveTrackers.ContainsKey(request))
        {
            error.AppendLine("The request is already active thus can't be moved from pending to active.");
        }

        TResult result = new();

        if (error.Length > 0)
        {
            result.WithError(error.ToString());
            return result;
        }

        ActiveTrackers.Add(request, PendingTrackers[request]);
        PendingTrackers.Remove(request);

        result.WithSuccess("Moved request from pending to active.");
        return result;
    }

    /// <summary>
    /// Tries to get the tracker of an active request.
    /// </summary>
    internal RequestTracker Get(IBaseRequest request)
    {
        if (PendingTrackers.ContainsKey(request))
        {
            return PendingTrackers[request];
        }

        if (ActiveTrackers.ContainsKey(request))
        {

            return ActiveTrackers[request];
        }

        throw new RequestTrackerNotFoundException(request);
    }

    /// <summary>
    /// Get the tracker of an active request.
    /// </summary>
    internal bool TryGetActive(IBaseRequest request, out RequestTracker tracker)
    {
        if (PendingTrackers.ContainsKey(request))
        {
            return PendingTrackers.TryGetValue(request, out tracker);
        }
        return ActiveTrackers.TryGetValue(request, out tracker);
    }

    /// <summary>
    /// Try and remove a request tracker. This should be called when the request has been completed.
    /// </summary>
    internal bool TryRemove(IBaseRequest request)
    {
        // Just in case an error occurred such the request was never made active.
        if (PendingTrackers.ContainsKey(request))
        {
            PendingTrackers.Remove(request);
        }

        if (ActiveTrackers.ContainsKey(request))
        {
            ActiveTrackers.Remove(request);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks the active request handlers with a request that satisfies <paramref name="constraints"/>.
    /// </summary>
    /// <returns>The amount of active handlers that have request satisfying <paramref name="constraints"/>.</returns>
    internal int ActiveCount<TRequest>(Func<TRequest, bool> constraints) =>
        ActiveTrackers.Where(t => t.Key.GetType() == typeof(TRequest))
            .Where(t => constraints((TRequest)t.Key))
            .Count();
}