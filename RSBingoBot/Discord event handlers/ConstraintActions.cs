// <copyright file="ConstraintActions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordEventHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.EventArgs;

    /// <summary>
    /// Maps constraints to actions.
    /// </summary>
    /// <typeparam name="TConstraint">Type of constraint.</typeparam>
    /// <typeparam name="TArgs">Type of <see cref="DiscordEventArgs"/> used in the event callback.</typeparam>
    public class ConstraintActions<TConstraint, TArgs> where TConstraint : notnull
                                                       where TArgs : DiscordEventArgs
    {
        private readonly Dictionary<TConstraint, List<Func<DiscordClient, TArgs, Task>>> constraintToActions = new ();
        private readonly List<Func<DiscordClient, TArgs, Task>> nullConstraintActions = new ();

        /// <summary>
        /// Add an action with its constraint.
        /// </summary>
        /// <typeparam name="TArgs">The <see cref="DiscordEventArgs"/> type required in the event callback.</typeparam>
        /// <param name="constraints">
        /// The value the constraint must be for the <paramref name="action"/> to be able to be called when the event is invoked.
        /// If this is null, it will always be satisfied, thus <see cref="GetActions(TConstraint)"/> will always return the <paramref name="action"/>.
        /// </param>
        /// <param name="action">The action to be called.</param>
        public void Add(TConstraint? constraints, Func<DiscordClient, TArgs, Task> action)
        {
            if (constraints == null)
            {
                nullConstraintActions.Add(action);
                return;
            }

            if (constraintToActions.ContainsKey(constraints))
            {
                constraintToActions[constraints].Add(action);
            }
            else
            {
                constraintToActions.Add(constraints, new () { action });
            }
        }

        /// <summary>
        /// Remove the <paramref name="action"/> if it has been added with <paramref name="constraints"/>.
        /// </summary>
        /// <param name="constraints">The constraints the action was added with.</param>
        /// <param name="action">The action to remove.</param>
        public void Remove(TConstraint? constraints, Func<DiscordClient, TArgs, Task> action)
        {
            if (constraints == null)
            {
                if (nullConstraintActions.Contains(action))
                {
                    nullConstraintActions.Remove(action);
                }
                return;
            }

            if (constraintToActions.ContainsKey(constraints) &&
                constraintToActions[constraints].Contains(action))
            {
                constraintToActions[constraints].Remove(action);
            }
        }

        /// <summary>
        /// Get a list of actions satisfied by the given <paramref name="constraint"/>.
        /// </summary>
        /// <param name="constraint">The constraint to be satisfied.</param>
        /// <returns>The list of actions.</returns>
        public List<Func<DiscordClient, TArgs, Task>> GetActions(TConstraint constraint)
        {
            if (constraint != null && constraintToActions.ContainsKey(constraint))
            {
                return constraintToActions[constraint].Concat(nullConstraintActions).ToList();
            }
            return nullConstraintActions;
        }
    }
}
