// <copyright file="MessageCreatedDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord_event_handlers
{
    using System.Collections.Generic;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using RSBingoBot.Interfaces;

    /// <summary>
    /// Handles which subscribers to call when the <see cref="DiscordClient.MessageCreatedDEH"/> event is fired, based off given constraints.
    /// </summary>
    public class MessageCreatedDEH : DiscordEventHandlerBase<MessageCreateEventArgs, MessageCreatedDEH.Constraints>
    {
        public record Constraints : ConstraintsBase
        {
            /// <summary>
            /// Gets or sets the <see cref="DiscordChannel"/> that the message must be sent in.
            /// </summary>
            public DiscordChannel? Channel { get; set; } = null;

            /// <summary>
            /// Gets or sets the <see cref="DiscordUser"/> that the message must be sent by.
            /// </summary>
            public DiscordUser? Author { get; set; } = null;

            /// <summary>
            /// Gets or sets the number of attachments that the message must contain.
            /// </summary>
            public int? NumberOfAttachments { get; set; } = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="Constraints"/> class.
            /// </summary>
            /// <param name="channel">Gets or sets the <see cref="DiscordChannel"/> that the message must be sent in.</param>
            /// <param name="author">Gets or sets the <see cref="DiscordUser"/> that the message must be sent by.</param>
            /// <param name="numberOfAttachments">Gets or sets the number of attachments that the message must contain.</param>
            public Constraints(DiscordChannel? channel = null, DiscordUser? author = null, int? numberOfAttachments = null)
            {
                Channel = channel;
                Author = author;
                NumberOfAttachments = numberOfAttachments;
            }
        }

        /// <inheritdoc/>
        public override List<object> GetConstraintValues(Constraints constriants) =>
            new () { constriants.Channel, constriants.Author, constriants.NumberOfAttachments };

        /// <inheritdoc/>
        public override List<object> GetArgValues(MessageCreateEventArgs args) =>
            new () { args.Channel, args.Author, args.Message.Attachments.Count };
    }

}

