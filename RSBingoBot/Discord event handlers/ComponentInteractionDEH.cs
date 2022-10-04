// <copyright file="ComponentInteractionDEH.cs" company="PlaceholderCompany">
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
    public class ComponentInteractionDEH
    {
        public record Constraints(DiscordChannel? Channel = null, DiscordUser? User = null, string CustomId = "");

        /// <summary>
        /// The channel constrains.
        /// </summary>
        protected static readonly ConstraintActions<DiscordChannel, ComponentInteractionCreateEventArgs> ChannelConstraints = new ();

        /// <summary>
        /// The user constraints.
        /// </summary>
        protected static readonly ConstraintActions<DiscordUser, ComponentInteractionCreateEventArgs> UserConstraints = new ();

        /// <summary>
        /// The custom id constraints.
        /// </summary>
        protected static readonly ConstraintActions<string, ComponentInteractionCreateEventArgs> CustomIdConstraints = new ();

        /// <summary>
        /// Subscribe to the event. When it is fired and the <paramref name="constraints"/> are satisfied,
        /// <paramref name="callback"/> is called.
        /// </summary>
        /// <param name="constraints">The constraints to be satisfied.</param>
        /// <param name="callback">The action to call.</param>
        public static void Subscribe(Constraints constraints, Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> callback)
        {
            ChannelConstraints.Add(constraints.Channel, callback);
            UserConstraints.Add(constraints.User, callback);
            CustomIdConstraints.Add(constraints.CustomId, callback);
        }

        /// <summary>
        /// Unsubscribe from the event so the <paramref name="callback"/> is not longer called when the event is fired.
        /// </summary>
        /// <param name="constraints">The constraints the <paramref name="callback"/> was registered with.</param>
        /// <param name="callback">The action to stop being called.</param>
        public static void UnSubscribe(Constraints constraints, Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> callback)
        {
            ChannelConstraints.Remove(constraints.Channel, callback);
            UserConstraints.Remove(constraints.User, callback);
            CustomIdConstraints.Remove(constraints.CustomId, callback);
        }

        /// <summary>
        /// Called when the ComponentInteractionCreated event is fired.
        /// </summary>
        /// <param name="client">The <see cref="DiscordClient"/> the event was fired on.</param>
        /// <param name="args">The args for the event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task OnEvent(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            IEnumerable<Func<DiscordClient, ComponentInteractionCreateEventArgs, Task>> actionsToCall =
                ChannelConstraints.GetActions(args.Channel).Intersect(
                UserConstraints.GetActions(args.User).Intersect(
                CustomIdConstraints.GetActions(args.Interaction.Data.CustomId)));

            for (int i = actionsToCall.Count() - 1; i > -1; i--)
            {
                await actionsToCall.ElementAt(i)(client, args);
            }
        }
    }
}
