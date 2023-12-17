// <copyright file="DiscordTeamChannelsInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class DiscordTeamChannelsInfo
{
    public ChannelInfo Category { get; }
    public ChannelInfo Board { get; }
    public ChannelInfo General { get; }
    public ChannelInfo Evidence { get; }
    public ChannelInfo Voice { get; }

    /// <summary>
    /// Ensure the <paramref name="discordTeam"/>'s <see cref="RSBingoBot.Discord.DiscordTeam.Role"/> is not null.<br/>
    /// Ensure the <paramref name="discordTeam"/>'s <see cref="RSBingoBot.Discord.DiscordTeam.CategoryChannel"/> is not null before creating other channels.
    /// </summary>
    public DiscordTeamChannelsInfo(RSBingoBot.Discord.DiscordTeam discordTeam)
    {
        if (discordTeam.Role is null)
        {
            throw new ArgumentNullException(nameof(discordTeam.Role));
        }

        Category = new(discordTeam.Team.Name, ChannelType.Category, Overwrites:
            GetOverwrites(discordTeam.Role, Permissions.None, Permissions.AccessChannels));

        Board = new($"{discordTeam.Team.Name}-board", ChannelType.Text, discordTeam.CategoryChannel,
            GetOverwrites(discordTeam.Role, Permissions.SendMessages | Permissions.SendMessagesInThreads, Permissions.SendMessages));

        General = new($"{discordTeam.Team.Name}-general", ChannelType.Text, discordTeam.CategoryChannel,
            GetOverwrites(discordTeam.Role, Permissions.AccessChannels, Permissions.AccessChannels));

        Evidence = new($"{discordTeam.Team.Name}-evidence", ChannelType.Text, discordTeam.CategoryChannel,
            GetOverwrites(discordTeam.Role, Permissions.AccessChannels | Permissions.SendMessages | Permissions.SendMessagesInThreads,
                Permissions.AccessChannels));

        Voice = new($"{discordTeam.Team.Name}-voice", ChannelType.Voice, discordTeam.CategoryChannel,
            GetOverwrites(discordTeam.Role, Permissions.AccessChannels, Permissions.AccessChannels));
    }

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