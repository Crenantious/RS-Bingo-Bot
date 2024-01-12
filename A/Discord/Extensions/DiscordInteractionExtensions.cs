// <copyright file="DiscordInteractionExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordExtensions;

using DSharpPlus.Entities;

public static class DiscordInteractionExtensions
{
    public static async Task<bool> HasResponse(this DiscordInteraction interaction)
    {
        // TODO: JR - check the specific exception.
        try
        {
            await interaction.GetOriginalResponseAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}