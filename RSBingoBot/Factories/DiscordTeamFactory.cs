// <copyright file="DiscordTeamFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Factories;

using DiscordLibrary.DiscordServices;
using RSBingo_Framework.Models;
using RSBingoBot.DTO;

// This will be called from somewhere that will null check everything then run request to create missing content.
public class DiscordTeamFactory
{
    private readonly IDiscordChannelServices discordChannelServices;
    private readonly IDiscordMessageServices discordMessageServices;

    public DiscordTeamFactory(IDiscordChannelServices discordChannelServices, IDiscordMessageServices discordMessageServices)
    {
        this.discordChannelServices = discordChannelServices;
        this.discordMessageServices = discordMessageServices;
    }

    public async Task<DiscordTeam> Create(Team team)
    {
        var boardChannel = discordChannelServices.Get(team.BoardChannelId);
        var categoryChannel = discordChannelServices.Get(team.CategoryChannelId);
        var generalChannel = discordChannelServices.Get(team.GeneralChannelId);
        var evidenceChannel = discordChannelServices.Get(team.EvidenceChannelId);
        var voiceChannel = discordChannelServices.Get(team.VoiceChannelId);
        return new DiscordTeam(boardChannel, categoryChannel, boardChannel,
            generalChannel, evidenceChannel, await discordMessageServices.Get(team.BoardMessageId, boardChannel));
    }
}