// <copyright file="ModalSubmittedDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord_event_handlers
{
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    /// <summary>
    /// Handles which subscribers to call when the <see cref="DiscordClient.ModalSubmitted"/> event is fired, based off given constraints.
    /// </summary>
    public class ModalSubmittedDEH : DiscordEventHandlerBase<ModalSubmitEventArgs, ModalSubmittedDEH.Constraints>
    {
        public record Constraints : ConstraintsBase
        {
            /// <summary>
            /// Gets or sets the <see cref="DiscordUser"/> that must submit the modal.
            /// </summary>
            public DiscordUser? User { get; set; } = null;

            /// <summary>
            /// Gets or sets the custom id the submitted modal must have.
            /// </summary>
            public string? CustomId { get; set; } = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="Constraints"/> class.
            /// </summary>
            /// <param name="user">The <see cref="DiscordUser"/> that must submit the modal.</param>
            /// <param name="customId">The custom id the submitted modal must have.</param>
            public Constraints(DiscordUser? user = null, string? customId = null)
            {
                User = user;
                CustomId = customId;
            }
        }

        /// <inheritdoc/>
        public override List<object> GetConstraintValues(Constraints constriants) =>
            new () { constriants.User, constriants.CustomId };

        /// <inheritdoc/>
        public override List<object> GetArgValues(ModalSubmitEventArgs args) =>
            new () { args.Interaction.User, args.Interaction.Data.CustomId };
    }
}
