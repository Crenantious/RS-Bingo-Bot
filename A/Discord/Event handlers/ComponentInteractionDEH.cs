// <copyright file="ComponentInteractionDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEventHandlers;

using DSharpPlus.EventArgs;

public class ComponentInteractionDEH : DiscordEventHandlerBase<ComponentInteractionCreateEventArgs>
{
    public ComponentInteractionDEH() =>
        Client.ComponentInteractionCreated += OnEvent;
}