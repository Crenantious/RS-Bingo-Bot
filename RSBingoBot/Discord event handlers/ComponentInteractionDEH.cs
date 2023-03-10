// <copyright file="ComponentInteractionDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord_event_handlers;

using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RSBingoBot.Interfaces;

/// <summary>
/// Handles which subscribers to call when the <see cref="DiscordClient.ComponentInteractionCreated"/> event is fired based off given constraints.
/// </summary>
public class ComponentInteractionDEH : DiscordEventHandlerBase<ComponentInteractionCreateEventArgs, ComponentInteractionDEH.Constraints>
{
    public record Constraints(DiscordChannel? channel = null, DiscordUser? user = null, string? customId = null);

    public ComponentInteractionDEH() =>
        DiscordClient.ComponentInteractionCreated += OnEvent;

    /// <inheritdoc/>
    public override List<object> GetConstraintValues(Constraints constriants) =>
        new () { constriants.channel!, constriants.user!, constriants.customId! };

    /// <inheritdoc/>
    public override List<object> GetArgValues(ComponentInteractionCreateEventArgs args) =>
        new () { args.Channel, args.User, args.Interaction.Data.CustomId };
}