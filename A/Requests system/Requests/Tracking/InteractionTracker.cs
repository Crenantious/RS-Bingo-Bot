// <copyright file="InteractionTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using RSBingo_Common;

public class InteractionTracker<TRequest> : IInteractionTracker
    where TRequest : IInteractionRequest
{
    public int Id { get; }
    public TRequest Request { get; }
    public DiscordInteraction Interaction { get; }

    public InteractionTracker(TRequest request, DiscordInteraction interaction)
    {
        Id = IInteractionTracker.CurrentId++;
        Request = request;
        Interaction = interaction;
    }

    public async Task ConcludeInteraction()
    {
        var interactionTrackers = General.DI.Get<InteractionsTracker>();
        interactionTrackers.TryRemove(this);
    }
}