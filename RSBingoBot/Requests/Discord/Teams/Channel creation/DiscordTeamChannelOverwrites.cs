// <copyright file="DiscordTeamChannelOverwrites.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class DiscordTeamChannelOverwrites
{
    public DiscordOverwriteBuilder[] GetCategory(DiscordRole teamRole) =>
        GetOverwrites(teamRole, Permissions.None, Permissions.AccessChannels);

    public DiscordOverwriteBuilder[] GetBoard(DiscordRole teamRole) =>
        GetOverwrites(teamRole, Permissions.SendMessages | Permissions.SendMessagesInThreads, Permissions.SendMessages);

    public DiscordOverwriteBuilder[] GetGeneral(DiscordRole teamRole) =>
        GetOverwrites(teamRole, Permissions.AccessChannels, Permissions.AccessChannels);

    public DiscordOverwriteBuilder[] GetEvidence(DiscordRole teamRole) =>
        GetOverwrites(teamRole, Permissions.AccessChannels | Permissions.SendMessages | Permissions.SendMessagesInThreads,
            Permissions.AccessChannels);

    public DiscordOverwriteBuilder[] GetVoice(DiscordRole teamRole) =>
        GetOverwrites(teamRole, Permissions.AccessChannels, Permissions.AccessChannels);

    private static DiscordOverwriteBuilder[] GetOverwrites(DiscordRole teamRole,
        Permissions denyEveryonePermissions, Permissions allowTeamPermissions) =>
        new DiscordOverwriteBuilder[]
            {
                new(DataFactory.Guild.EveryoneRole)
                {
                    Denied = denyEveryonePermissions,
                },
                new(teamRole)
                {
                    Allowed = allowTeamPermissions,
                }
            };
}