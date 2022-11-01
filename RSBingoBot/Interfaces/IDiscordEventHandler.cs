// <copyright file="IDiscordEventHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces
{
    using DSharpPlus;
    using DSharpPlus.EventArgs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDiscordEventHandler<TEventArgs, TConstraints>
    {
        /// <summary>
        /// Subscribe to the event. When it is fired and the <paramref name="constraints"/> are satisfied,
        /// <paramref name="callback"/> is called.
        /// </summary>
        /// <param name="constraints">The constraints to be satisfied.</param>
        /// <param name="callback">The action to call.</param>
        public void Subscribe(TConstraints constraints, Func<DiscordClient, TEventArgs, Task> callback);

        /// <summary>
        /// Unsubscribe from the event so the <paramref name="callback"/> is not longer called when the event is fired.
        /// </summary>
        /// <param name="constraints">The constraints the <paramref name="callback"/> was registered with.</param>
        /// <param name="callback">The action to stop being called.</param>
        public void UnSubscribe(TConstraints constraints, Func<DiscordClient, TEventArgs, Task> callback);

        /// <summary>
        /// Called when the Discord event is fired.
        /// </summary>
        /// <param name="client">The <see cref="DiscordClient"/> the event was fired on.</param>
        /// <param name="args">The args for the event.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task OnEvent(DiscordClient client, TEventArgs args);

    }
}
