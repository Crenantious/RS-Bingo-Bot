// <copyright file="DiscordEventHandlerBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEventHandlers;

using DSharpPlus;
using DSharpPlus.EventArgs;
using RSBingo_Common;

/// <summary>
/// Handles which subscribers to call when the appropriate Discord event is fired, based off given constraints.
/// </summary>
public abstract class DiscordEventHandlerBase<TEventArgs>
    where TEventArgs : DiscordEventArgs
{
    private static int id = 0;

    private readonly List<DiscordEventHandlerSubscription<TEventArgs>> subscriptions = new();
    private readonly Dictionary<int, DiscordEventHandlerSubscription<TEventArgs>> idToSubscription = new();

    private bool isEventOccuring = false;
    private List<DiscordEventHandlerSubscription<TEventArgs>> queuedToSubscribe = new();
    private List<int> queuedToUnSubscribe = new();

    protected static DiscordClient Client { get; } = (DiscordClient)General.DI.GetService(typeof(DiscordClient))!;

    #region Subscribe

    /// <summary>
    /// Subscribe to the event. When it is fired and <paramref name="constraints"/> are satisfied,
    /// <paramref name="callback"/> is called.
    /// </summary>
    /// <returns>The id used to unsubscribe.</returns>
    public int Subscribe(Func<TEventArgs, bool> constraints, Func<TEventArgs, Task> callback)
    {
        DiscordEventHandlerSubscription<TEventArgs> subscription = new(id++, constraints, callback);
        if (isEventOccuring)
        {
            QueueSubscription(subscription);
        }
        else
        {
            Subscribe(subscription);
        }
        return subscription.Id;
    }

    private void QueueSubscription(DiscordEventHandlerSubscription<TEventArgs> subscription)
    {
        queuedToSubscribe.Add(subscription);
    }

    private void Subscribe(DiscordEventHandlerSubscription<TEventArgs> subscription)
    {
        subscriptions.Add(subscription);
        idToSubscription.Add(subscription.Id, subscription);
    }

    #endregion

    #region Unsubscribe

    public void PrivateUnsubscribe(int id)
    {
        if (idToSubscription.ContainsKey(id) is false)
        {
            throw new InvalidEventSubscriptionIdException(id);
        }

        if (isEventOccuring)
        {
            QueueToUnsubscribe(id);
        }
        else
        {
            Unsubscribe(id);
        }
    }

    private void QueueToUnsubscribe(int id)
    {
        queuedToUnSubscribe.Add(id);
    }

    private void Unsubscribe(int id)
    {
        subscriptions.Remove(idToSubscription[id]);
        idToSubscription.Remove(id);
    }

    #endregion

    /// <summary>
    /// Called when the Discord event is fired.
    /// </summary>
    /// <param name="client">The <see cref="DSharpPlus.DiscordClient"/> the event was fired on.</param>
    /// <param name="args">The args for the event.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task OnEvent(DiscordClient client, TEventArgs args)
    {
        isEventOccuring = true;

        foreach (var subscription in subscriptions)
        {
            await subscription.OnEvent(args);
        }

        isEventOccuring = false;
        SubscribeQueued();
        UnsubscribeQueued();
    }

    private void UnsubscribeQueued()
    {
        foreach (int id in queuedToUnSubscribe)
        {
            PrivateUnsubscribe(id);
        }

        queuedToUnSubscribe.Clear();
    }

    private void SubscribeQueued()
    {
        foreach (var subscription in queuedToSubscribe)
        {
            Subscribe(subscription);
        }

        queuedToSubscribe.Clear();
    }
}