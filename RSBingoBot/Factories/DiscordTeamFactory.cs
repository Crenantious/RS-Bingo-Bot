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
    private readonly IDiscordServices discordServices;
    private readonly IDiscordMessageServices discordMessageServices;

    public DiscordTeamFactory(IDiscordServices discordChannelServices, IDiscordMessageServices discordMessageServices)
    {
        this.discordServices = discordChannelServices;
        this.discordMessageServices = discordMessageServices;
    }

    public async Task<DiscordTeam> Create(Team team)
    {
        var boardChannel = await discordServices.GetChannel(team.BoardChannelId);
        var categoryChannel = await discordServices.GetChannel(team.CategoryChannelId);
        var generalChannel = await discordServices.GetChannel(team.GeneralChannelId);
        var evidenceChannel = await discordServices.GetChannel(team.EvidenceChannelId);
        var voiceChannel = await discordServices.GetChannel(team.VoiceChannelId);
        var role = await discordServices.GetRole(team.RoleId);
        var boardMessage = boardChannel.IsSuccess ?
            (await discordServices.GetMessage(team.BoardMessageId, boardChannel.Value)).Value
            : null;

        return new DiscordTeam(categoryChannel.Value, generalChannel.Value, boardChannel.Value,
            evidenceChannel.Value, voiceChannel.Value, boardMessage, role.Value);
    }
}