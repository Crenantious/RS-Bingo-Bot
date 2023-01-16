// <copyright file="MessageReactionAddedDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord_event_handlers
{
    using System;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using Emzi0767.Utilities;
    using RSBingoBot.Exceptions;
    using RSBingoBot.Interfaces;

    /// <summary>
    /// Handles which subscribers to call when the <see cref="DiscordClient.MessageReactionAdded"/> event is fired based off given constraints.
    /// </summary>
    public class MessageReactionAddedDEH : DiscordEventHandlerBase<MessageReactionAddEventArgs, MessageReactionAddedDEH.Constraints>
    {
        public record Constraints(DiscordChannel? channel = null, DiscordUser? user = null, string? emojiName = null);

        public MessageReactionAddedDEH() =>
            DiscordClient.MessageReactionAdded += OnEvent;

        /// <inheritdoc/>
        public override List<object> GetConstraintValues(Constraints constriants) =>
            new () { constriants.channel, constriants.user, constriants.emojiName };

        /// <inheritdoc/>
        public override List<object> GetArgValues(MessageReactionAddEventArgs args) =>
            new () { args.Channel, args.User, args.Emoji.Name };
    }
}