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
        public record Constraints(DiscordChannel? channel = null, DiscordUser? author = null,
            int? numberOfAttachments = null, string? attatchmentFileExtension = null);

        public MessageCreatedDEH() =>
            DiscordClient.MessageCreated += OnEvent;

        /// <inheritdoc/>
        public override List<object> GetConstraintValues(Constraints constriants) =>
            new () { constriants.channel, constriants.author,
                constriants.numberOfAttachments, constriants.attatchmentFileExtension};

        /// <inheritdoc/>
        public override List<object> GetArgValues(MessageCreateEventArgs args)
        {
            string? fileExtension = null;

            if (args.Message.Attachments.Count > 0)
            {
                fileExtension = args.Message.Attachments[0].MediaType;
                //string[] split = args.Message.Attachments[0].FileName.Split(".");
                //if (split.Length > 1)
                //{
                //    fileExtension = split[1];
                //}
            }
            return new List<object>() { args.Channel, args.Author, args.Message.Attachments.Count, fileExtension };
        }
    }
}

