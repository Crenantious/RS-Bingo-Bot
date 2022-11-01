// <copyright file="ComponentInteractionDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord_event_handlers
{
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    /// <summary>
    /// Handles which subscribers to call when the <see cref="DiscordClient.ComponentInteractionCreated"/> event is fired based off given constraints.
    /// </summary>
    public class ComponentInteractionDEH : DiscordEventHandlerBase<ComponentInteractionCreateEventArgs, ComponentInteractionDEH.Constraints>
    {
        public record Constraints : ConstraintsBase
        {
            /// <summary>
            /// Gets or sets the <see cref="DiscordChannel"/> that the component must be in.
            /// </summary>
            public DiscordChannel? Channel { get; set; } = null;

            /// <summary>
            /// Gets or sets the <see cref="DiscordUser"/> that must have interacted with the component.
            /// </summary>
            public DiscordUser? User { get; set; } = null;

            /// <summary>
            /// Gets or sets the custom id that the component must have.
            /// </summary>
            public string? CustomId { get; set; } = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="Constraints"/> class.
            /// </summary>
            /// <param name="channel">Gets or sets the <see cref="DiscordChannel"/> that the component must be in.</param>
            /// <param name="user">Gets or sets the <see cref="DiscordUser"/> that must have interacted with the component.</param>
            /// <param name="customId">Gets or sets the custom id that the component must have.</param>
            public Constraints(DiscordChannel? channel = null, DiscordUser? user = null, string? customId = null)
            {
                Channel = channel;
                User = user;
                CustomId = customId;
            }
        }

        /// <inheritdoc/>
        public override List<object> GetConstraintValues(Constraints constriants) =>
            new () { constriants.Channel, constriants.User, constriants.CustomId };

        /// <inheritdoc/>
        public override List<object> GetArgValues(ComponentInteractionCreateEventArgs args) =>
            new () { args.Channel, args.User, args.Interaction.Data.CustomId };

    }
}