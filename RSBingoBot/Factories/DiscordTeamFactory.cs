// <copyright file="DiscordTeamFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Factories;

using DiscordLibrary.DiscordServices;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

public class DiscordTeamFactory
{
    private readonly IDiscordTeamServices discordServices;

    public DiscordTeamFactory(IDiscordTeamServices teamServices)
    {
        this.discordServices = teamServices;
    }

    public async Task<DiscordTeam> CreateNew(string name)
    {
        Team team = new();
        team.Name = name;
        DiscordTeam discordTeam = new(team);
        await discordServices.CreateMissingEntities(discordTeam);
        return discordTeam;
    }

    public async Task<DiscordTeam> CreateFromExisting(Team team)
    {
        DiscordTeam discordTeam = new(team);
        await discordServices.SetExistingEntities(discordTeam);
        return discordTeam;
    }
}