// <copyright file="ModalSubmittedDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord_event_handlers
{
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using RSBingoBot.Interfaces;

    /// <summary>
    /// Handles which subscribers to call when the <see cref="DiscordClient.ModalSubmitted"/> event is fired, based off given constraints.
    /// </summary>
    public class ModalSubmittedDEH : DiscordEventHandlerBase<ModalSubmitEventArgs, ModalSubmittedDEH.Constraints>
    {
        public record Constraints(DiscordUser? user = null, string? customId = null);

        public ModalSubmittedDEH() =>
            DiscordClient.ModalSubmitted += OnEvent;

        /// <inheritdoc/>
        public override List<object> GetConstraintValues(Constraints constriants) =>
            new () { constriants.user, constriants.customId };

        /// <inheritdoc/>
        public override List<object> GetArgValues(ModalSubmitEventArgs args) =>
            new () { args.Interaction.User, args.Interaction.Data.CustomId };
    }
}
