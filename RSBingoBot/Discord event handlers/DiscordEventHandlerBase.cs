// <copyright file="DiscordEventHandlerBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord_event_handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.Logging;
    using static RSBingo_Common.General;

    /// <summary>
    /// Handles which subscribers to call when the appropriate Discord event is fired, based off given constraints.
    /// </summary>
    /// <typeparam name="TEventArgs">The args given when the Discord event is fired.</typeparam>
    /// <typeparam name="TConstraints">The derived class's constraints, inherited from <see cref="ConstraintsBase"/>.</typeparam>
    public abstract class DiscordEventHandlerBase<TEventArgs, TConstraints>
        where TEventArgs : DiscordEventArgs
        where TConstraints : DiscordEventHandlerBase<TEventArgs, TConstraints>.ConstraintsBase
    {
        private readonly List<ConstraintActions<object, TEventArgs>> constraintActions = new ();

        /// <summary>
        /// The base record for all Constraint records in derived classes.
        /// </summary>
        public record ConstraintsBase();

        /// <summary>
        /// Gets each property of the <paramref name="constraints"/> in order.
        /// </summary>
        /// <param name="constraints">The constraints to be parsed.</param>
        /// <returns>The list of properties in order.</returns>
        public abstract List<object> GetConstraintValues(TConstraints constraints);

        /// <summary>
        /// Gets each constraint value of the <paramref name="args"/> in the order that
        /// corresponds the <see cref="GetConstraintValues"/> method.
        /// </summary>
        /// <param name="args">The args to parse.</param>
        /// <returns>The list of values in order.</returns>
        public abstract List<object> GetArgValues(TEventArgs args);

        /// <summary>
        /// Subscribe to the event. When it is fired and the <paramref name="constraints"/> are satisfied,
        /// <paramref name="callback"/> is called.
        /// </summary>
        /// <param name="constraints">The constraints to be satisfied.</param>
        /// <param name="callback">The action to call.</param>
        public void Subscribe(TConstraints constraints, Func<DiscordClient, TEventArgs, Task> callback)
        {
            if (constraintActions.Count == 0)
            {
                for (int i = 0; i < GetConstraintValues(constraints).Count; i++)
                {
                    constraintActions.Add(new ConstraintActions<object, TEventArgs>());
                }
            }

            var values = GetConstraintValues(constraints);

            for (int i = 0; i < values.Count; i++)
            {
                constraintActions[i].Add(values[i], callback);
            }
        }

        /// <summary>
        /// Unsubscribe from the event so the <paramref name="callback"/> is not longer called when the event is fired.
        /// </summary>
        /// <param name="constraints">The constraints the <paramref name="callback"/> was registered with.</param>
        /// <param name="callback">The action to stop being called.</param>
        public void UnSubscribe(TConstraints constraints, Func<DiscordClient, TEventArgs, Task> callback)
        {
            if (constraintActions.Count == 0) { return; }

            var values = GetConstraintValues(constraints);

            for (int i = 0; i < values.Count; i++)
            {
                constraintActions[i].Remove(values[i], callback);
            }
        }

        /// <summary>
        /// Called when the Discord event is fired.
        /// </summary>
        /// <param name="client">The <see cref="DiscordClient"/> the event was fired on.</param>
        /// <param name="args">The args for the event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnEvent(DiscordClient client, TEventArgs args)
        {
            if (constraintActions.Count == 0) { return; }

            List<object>? argValues = GetArgValues(args);
            List<Func<DiscordClient, TEventArgs, Task>> intersectingActions = new (constraintActions[0].GetActions(argValues[0]));

            for (int i = 1; i < argValues.Count; i++)
            {
                intersectingActions = intersectingActions.Intersect(constraintActions[i].GetActions(argValues[i])).ToList();
            }

            foreach (var action in intersectingActions)
            {
                try
                {
                    await action(client, args);
                }
                catch (Exception e)
                {
                    ILogger<ComponentInteractionDEH> logger = LoggingInstance<ComponentInteractionDEH>();
                    LoggingLog(e, e.Message);
                }
            }
        }
    }
}
