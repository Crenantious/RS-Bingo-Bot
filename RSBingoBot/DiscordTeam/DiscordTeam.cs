// <copyright file="DiscordTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;

public class DiscordTeam
{
    public Team Team { get; }
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