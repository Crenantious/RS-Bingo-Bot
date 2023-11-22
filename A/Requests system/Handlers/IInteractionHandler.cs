// <copyright file="IInteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.RequestHandlers;

public interface IInteractionHandler
{
    /// <summary>
    /// Conclude the interaction such that the user can no longer interact
    /// with any <see cref="Interactable"/> created by this handler.
    /// </summary>
    public Task Conclude();
}