// <copyright file="DiscordRequestServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordServices;

using DSharpPlus.Entities;

public class DiscordRequestServices
{
    /// <summary>
    /// Attempts to send a request to Discord. If the request fails through a web error it will be retried a number of times.
    /// </summary>
    /// <returns><see langword="true"/> if the request was successful, <see langword="false"/> otherwise.</returns>
    public static async Task<bool> SendDiscordRequest(Func<Task> request)
    {
        // TODO: JR - make this auto retry if there was a non fatal error (i.e. a web exception).
        // Return false if the request is invalid (i.e. "invalid body form" in a message).
        try { await request.Invoke(); }
        catch { return false; }
        return true;
    }
}