// <copyright file="MessageCreatedDEH.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord_event_handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    /// <summary>
    /// Handles which subscribers to call when the ComponentInteractionCreated event is fired based off given constraints.
    /// </summary>
    public class MessageCreatedDEH
    {
        public record Constraints(DiscordChannel? Channel = null, DiscordUser? Author = null, int NumberOfAttachments = -1);

        /// <summary>
        /// The channel constrains.
        /// </summary>
        protected static readonly ConstraintActions<DiscordChannel, MessageCreateEventArgs> ChannelConstraints = new ();

        /// <summary>
        /// The author constraints.
        /// </summary>
        protected static readonly ConstraintActions<DiscordUser, MessageCreateEventArgs> AuthorConstraints = new ();

        /// <summary>
        /// The number of attachments constraints.
        /// </summary>
        protected static readonly ConstraintActions<int, MessageCreateEventArgs> NumberOfAttachmentsConstraints = new ();

        /// <summary>
        /// Subscribe to the event. When it is fired and the <paramref name="constraints"/> are satisfied,
        /// <paramref name="callback"/> is called.
        /// </summary>
        /// <param name="constraints">The constraints to be satisfied.</param>
        /// <param name="callback">The action to call.</param>
        public static void Subscribe(Constraints constraints, Func<DiscordClient, MessageCreateEventArgs, Task> callback)
        {
            ChannelConstraints.Add(constraints.Channel, callback);
            AuthorConstraints.Add(constraints.Author, callback);
            NumberOfAttachmentsConstraints.Add(constraints.NumberOfAttachments, callback);
        }

        /// <summary>
        /// Unsubscribe from the event so the <paramref name="callback"/> is no longer called when the event is fired.
        /// </summary>
        /// <param name="constraints">The constraints the <paramref name="callback"/> was registered with.</param>
        /// <param name="callback">The action to stop being called.</param>
        public static void UnSubscribe(Constraints constraints, Func<DiscordClient, MessageCreateEventArgs, Task> callback)
        {
            ChannelConstraints.Remove(constraints.Channel, callback);
            AuthorConstraints.Remove(constraints.Author, callback);
            NumberOfAttachmentsConstraints.Remove(constraints.NumberOfAttachments, callback);
        }

        /// <summary>
        /// Called when the <see cref="DiscordClient.MessageCreated"/> event is fired.
        /// </summary>
        /// <param name="client">The <see cref="DiscordClient"/> the event was fired on.</param>
        /// <param name="args">The args for the event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task OnEvent(DiscordClient client, MessageCreateEventArgs args)
        {
            IEnumerable<Func<DiscordClient, MessageCreateEventArgs, Task>>? actionsToCall =
                ChannelConstraints.GetActions(args.Channel).Intersect(
                AuthorConstraints.GetActions(args.Author).Intersect(
                NumberOfAttachmentsConstraints.GetActions(args.Message.Attachments.Count)));

            foreach (Func<DiscordClient, MessageCreateEventArgs, Task> action in actionsToCall)
            {
                await action(client, args);
            }
        }
    }
}
