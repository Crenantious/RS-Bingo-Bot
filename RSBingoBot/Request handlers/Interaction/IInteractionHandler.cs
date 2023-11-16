﻿// <copyright file="IInteractionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.InteractionHandlers;

using RSBingoBot.DiscordEntities;

internal interface IInteractionHandler
{
    /// <summary>
    /// Conclude the interaction such that the user can no longer interact
    /// with any <see cref="Interactable"/> created by this handler.
    /// </summary>
    public Task Conclude();

    /// <summary>
    /// Conclude the interaction and cleanup excess entities such as <see cref="Message"/>s.
    /// </summary>
    public Task Close();
}