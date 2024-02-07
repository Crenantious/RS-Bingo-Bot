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
    public enum Channel
    {
        Category,
        Board,
        General,
        Evidence,
        Voice,
    }

    /// <summary>
    /// Ensure the <paramref name="discordTeam"/>'s <see cref="RSBingoBot.Discord.DiscordTeam.Role"/> is not null.<br/>
    /// Ensure the <paramref name="discordTeam"/>'s <see cref="RSBingoBot.Discord.DiscordTeam.CategoryChannel"/> is not null before creating other channels.
    /// </summary>

    public ChannelInfo GetInfo(RSBingoBot.Discord.DiscordTeam discordTeam, Channel channel)
    {
        CheckRoleExists(discordTeam);

        return channel switch
        {
            Channel.Category => new(discordTeam.Name, ChannelType.Category, Overwrites:
                GetOverwrites(discordTeam.Role!, Permissions.None, Permissions.AccessChannels)),

            Channel.Board => new($"{discordTeam.Name}-board", ChannelType.Text, discordTeam.CategoryChannel,
                GetOverwrites(discordTeam.Role!, Permissions.SendMessages | Permissions.SendMessagesInThreads, Permissions.SendMessages)),

            Channel.General => new($"{discordTeam.Name}-general", ChannelType.Text, discordTeam.CategoryChannel,
                GetOverwrites(discordTeam.Role!, Permissions.AccessChannels, Permissions.AccessChannels)),

            Channel.Evidence => new($"{discordTeam.Name}-evidence", ChannelType.Text, discordTeam.CategoryChannel,
                GetOverwrites(discordTeam.Role!, Permissions.AccessChannels | Permissions.SendMessages | Permissions.SendMessagesInThreads,
                Permissions.AccessChannels)),

            Channel.Voice => new($"{discordTeam.Name}-voice", ChannelType.Voice, discordTeam.CategoryChannel,
                GetOverwrites(discordTeam.Role!, Permissions.AccessChannels, Permissions.AccessChannels)),

            _ => throw new ArgumentOutOfRangeException("")
        };
    }

    private static void CheckRoleExists(Discord.DiscordTeam discordTeam)
    {
        if (discordTeam.Role is null)
        {
            throw new ArgumentNullException(nameof(discordTeam.Role));
        }
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