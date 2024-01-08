// <copyright file="DiscordTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;

public class DiscordTeam
{
    public static Dictionary<string, DiscordTeam> ExistingTeams { get; } = new();

    // TODO: JR - check if Team can be used or if its id is required; data workers may need to get a
    // refreshed version since the team may have been created by a different dw and be out of sync.
    public Team Team { get; }
    public string RoleName => Team.Name;

    // TODO: JR - have a list of all channels to make it easy to manipulate (create, rename, delete etc.) all
    // channels together and make it very simple to add/remove channels from the list and have it affect all manipulators.
    public DiscordChannel? CategoryChannel { get; private set; }
    public DiscordChannel? GeneralChannel { get; private set; }
    public DiscordChannel? BoardChannel { get; private set; }
    public DiscordChannel? EvidenceChannel { get; private set; }
    public DiscordChannel? VoiceChannel { get; private set; }
    public Message? BoardMessage { get; private set; }
    public DiscordRole? Role { get; private set; }

    public DiscordTeam(Team team)
    {
        Team = team;
    }

    public void SetCategoryChannel(DiscordChannel channel)
    {
        CategoryChannel = channel;
        Team.CategoryChannelId = channel.Id;
    }

    public void SetBoardChannel(DiscordChannel channel)
    {
        BoardChannel = channel;
        Team.BoardChannelId = channel.Id;
    }

    public void SetGeneralChannel(DiscordChannel channel)
    {
        GeneralChannel = channel;
        Team.GeneralChannelId = channel.Id;
    }

    public void SetEvidenceChannel(DiscordChannel channel)
    {
        EvidenceChannel = channel;
        Team.EvidenceChannelId = channel.Id;
    }

    public void SetVoiceChannel(DiscordChannel channel)
    {
        VoiceChannel = channel;
        Team.VoiceChannelId = channel.Id;
    }

    public void SetBoardMessage(Message message)
    {
        BoardMessage = message;
        Team.BoardMessageId = message.DiscordMessage.Id;
    }

    public void SetRole(DiscordRole role)
    {
        Role = role;
        Team.RoleId = role.Id;
    }
}