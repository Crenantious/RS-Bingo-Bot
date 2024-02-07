// <copyright file="DiscordTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;

public class DiscordTeam
{
    // TODO: JR - move to a dedicated tracker.
    /// <summary>
    /// (Name, team).
    /// </summary>
    public static Dictionary<string, DiscordTeam> ExistingTeams { get; } = new();

    public string Name { get; private set; }

    /// <summary>
    /// The row id in the database.
    /// </summary>
    public int Id { get; }
    public string RoleName => Name;

    // TODO: JR - have a list of all channels to make it easy to manipulate (create, rename, delete etc.) all
    // channels together and make it very simple to add/remove channels from the list and have it affect all manipulators.
    public DiscordRole? Role { get; private set; }
    public DiscordChannel? CategoryChannel { get; private set; }
    public DiscordChannel? GeneralChannel { get; private set; }
    public DiscordChannel? BoardChannel { get; private set; }
    public DiscordChannel? EvidenceChannel { get; private set; }
    public DiscordChannel? VoiceChannel { get; private set; }
    public Message? BoardMessage { get; private set; }

    public DiscordTeam(Team team)
    {
        Name = team.Name;
        Id = team.RowId;
    }

    public void SetName(string name, Team team)
    {
        Name = name;
        team.Name = name;
    }

    public void SetRole(DiscordRole role, Team team)
    {
        Role = role;
        team.RoleId = role.Id;
    }

    public void SetCategoryChannel(DiscordChannel channel, Team team)
    {
        CategoryChannel = channel;
        team.CategoryChannelId = channel.Id;
    }

    public void SetBoardChannel(DiscordChannel channel, Team team)
    {
        BoardChannel = channel;
        team.BoardChannelId = channel.Id;
    }

    public void SetGeneralChannel(DiscordChannel channel, Team team)
    {
        GeneralChannel = channel;
        team.GeneralChannelId = channel.Id;
    }

    public void SetEvidenceChannel(DiscordChannel channel, Team team)
    {
        EvidenceChannel = channel;
        team.EvidenceChannelId = channel.Id;
    }

    public void SetVoiceChannel(DiscordChannel channel, Team team)
    {
        VoiceChannel = channel;
        team.VoiceChannelId = channel.Id;
    }

    public void SetBoardMessage(Message message, Team team)
    {
        BoardMessage = message;
        team.BoardMessageId = message.DiscordMessage.Id;
    }
}