// <copyright file="DiscordEventHandlerSubscription.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEventHandlers;

using DSharpPlus.EventArgs;

public class DiscordEventHandlerSubscription<TEventArgs> where TEventArgs : DiscordEventArgs
{
    private Func<TEventArgs, bool> constraints;
    private Func<TEventArgs, Task> callback;

    public int Id { get; }

    public DiscordEventHandlerSubscription(int id, Func<TEventArgs, bool> constraints, Func<TEventArgs, Task> callback)
    {
        Id = id;
        this.constraints = constraints;
        this.callback = callback;
    }

    public async Task OnEvent(TEventArgs args)
    {
        if (constraints(args))
        {
            await callback(args);
        }
    }
}