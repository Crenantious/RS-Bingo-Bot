// <copyright file="InteractionsTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.Exceptions;

public class InteractionsTracker
{
    private Dictionary<int, IInteractionTracker> trackers { get; } = new();
    private Dictionary<Type, List<IInteractionTracker>> trackersByType { get; } = new();

    internal void Add<TRequest>(InteractionTracker<TRequest> tracker)
        where TRequest : IInteractionRequest
    {
        trackers.Add(tracker.Id, tracker);

        Type requestType = typeof(TRequest);

        if (trackersByType.ContainsKey(requestType))
        {
            trackersByType[requestType].Add(tracker);
        }
        else
        {
            trackersByType.Add(requestType, new() { tracker });
        }
    }

    internal IInteractionTracker Get(int id)
    {
        if (trackers.ContainsKey(id))
        {
            return trackers[id];
        }
        return trackers[id];

        throw new InteractionTrackerNotFoundException(id);
    }

    /// <summary>
    /// Tries to get the tracker of an active interaction.
    /// </summary>
    internal bool TryGet(int id, out IInteractionTracker tracker) =>
        trackers.TryGetValue(id, out tracker);

    /// <summary>
    /// Try and remove an interaction tracker. This should be called when the interaction has concluded or erred opening to the user.
    /// </summary>
    internal bool TryRemove<TRequest>(InteractionTracker<TRequest> tracker)
        where TRequest : IInteractionRequest
    {
        if (trackers.ContainsKey(tracker.Id))
        {
            trackers.Remove(tracker.Id);
        }

        Type requestType = typeof(TRequest);

        if (trackersByType.ContainsKey(requestType))
        {
            trackersByType[requestType].Remove(tracker);
        }
        return false;
    }

    /// <summary>
    /// Checks the active interactions with a request that satisfies <paramref name="constraints"/>.
    /// </summary>
    /// <returns>The amount of active interactions that have request satisfying <paramref name="constraints"/>.</returns>
    internal int ActiveCount<TRequest>(Func<InteractionTracker<TRequest>, bool> constraints)
        where TRequest : IInteractionRequest
    {
        if (trackersByType.ContainsKey(typeof(TRequest)) is false)
        {
            return 0;
        }

        var a = trackersByType[typeof(TRequest)]
            .Where(t => constraints((InteractionTracker<TRequest>)t))
            .Count();
        return a;
    }
}