// <copyright file="InteractionTracker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

public interface IInteractionTracker
{
    internal static int CurrentId = 0;

    public int Id { get; }
    public DiscordInteraction Interaction { get; }

    public Task ConcludeInteraction();
}