// <copyright file="DiscordTeamChannelOverwrites.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

using DSharpPlus;
using DSharpPlus.Entities;

internal class DiscordTeamChannelOverwrites
{
    public DiscordOverwriteBuilder[] Category { get; }
    public DiscordOverwriteBuilder[] Board { get; }
    public DiscordOverwriteBuilder[] General { get; }
    public DiscordOverwriteBuilder[] Evidence { get; }
    public DiscordOverwriteBuilder[] Voice { get; }

    public DiscordTeamChannelOverwrites(DiscordGuild guild, DiscordRole teamRole)
    {
        Category = GetOverwrites(guild, teamRole, Permissions.None, Permissions.AccessChannels);
        Board = GetOverwrites(guild, teamRole, Permissions.SendMessages | Permissions.SendMessagesInThreads, Permissions.AccessChannels);
        General = GetOverwrites(guild, teamRole, Permissions.AccessChannels, Permissions.AccessChannels);
        Evidence = GetOverwrites(guild, teamRole, Permissions.AccessChannels | Permissions.SendMessages | Permissions.SendMessagesInThreads,
            Permissions.AccessChannels);
        Voice = GetOverwrites(guild, teamRole, Permissions.AccessChannels, Permissions.AccessChannels);
    }

    private static DiscordOverwriteBuilder[] GetOverwrites(DiscordGuild guild, DiscordRole teamRole,
        Permissions denyEveryonePermissions, Permissions allowTeamPermissions) =>
        new DiscordOverwriteBuilder[]
            {
                new(guild.EveryoneRole)
                {
                    Denied = denyEveryonePermissions,
                },
                new(teamRole)
                {
                    Allowed = allowTeamPermissions,
                }
            };
}