// <copyright file="DiscordTeamFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Factories;

using DiscordLibrary.DiscordServices;
using FluentResults;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

// TODO: JR - probably convert to requests.
public class DiscordTeamFactory
{
    private readonly IDiscordTeamServices discordServices;

    public DiscordTeamFactory(IDiscordTeamServices teamServices)
    {
        this.discordServices = teamServices;
        teamServices.Initialise(null);
    }

    public async Task<Result<DiscordTeam>> CreateNew(string name, IDataWorker dataWorker)
    {
        Team team = dataWorker.Teams.Create();
        team.Name = name;
        DiscordTeam discordTeam = new(team);

        Result result = await discordServices.CreateMissingEntities(discordTeam);

        return result.IsSuccess ?
            Result.Ok<DiscordTeam>(discordTeam) :
            Result.Fail<DiscordTeam>(result.Errors);
    }

    public async Task<Result<DiscordTeam>> CreateFromExisting(Team team)
    {
        DiscordTeam discordTeam = new(team);
        Result result = await discordServices.SetExistingEntities(discordTeam);
        return result.IsSuccess ?
            Result.Ok<DiscordTeam>(discordTeam) :
            Result.Fail<DiscordTeam>(result.Errors); ;
    }
}