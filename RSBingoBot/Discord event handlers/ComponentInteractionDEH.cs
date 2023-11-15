// <copyright file="ComponentInteractionDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordEventHandlers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

/// <summary>
/// Handles which subscribers to call when the <see cref="DiscordClient.ComponentInteractionCreated"/> event is fired based off given constraints.
/// </summary>
// TODO: JR - put in separate assembly and make Constraints internal.
public class ComponentInteractionDEH : DiscordEventHandlerBase<ComponentInteractionCreateEventArgs, ComponentInteractionDEH.Constraints>
{
    /// <summary>
    /// Contains only user-configurable options. This is used outside of where the registration takes place, e.g. a factory.
    /// </summary>
    public record StrippedConstraints(DiscordChannel? Channel = null, DiscordUser? User = null);

    /// <summary>
    /// Contains all options. This is used where the registration takes place, e.g. a factory.
    /// </summary>
    public record Constraints : StrippedConstraints
    {
        public string CustomId { get; }

        public Constraints(StrippedConstraints parent, string customId) : base(parent)
        {
            CustomId = customId;
        }
    }

    public ComponentInteractionDEH() =>
        DiscordClient.ComponentInteractionCreated += OnEvent;

    /// <inheritdoc/>
    public override List<object> GetConstraintValues(Constraints constriants) =>
        new() { constriants.Channel!, constriants.User!, constriants.CustomId! };

    /// <inheritdoc/>
    public override List<object> GetArgValues(ComponentInteractionCreateEventArgs args) =>
        new() { args.Channel, args.User, args.Interaction.Data.CustomId };
}