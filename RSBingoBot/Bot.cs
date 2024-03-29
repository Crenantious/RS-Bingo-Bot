﻿// <copyright file="Bot.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using DSharpPlus;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.BingoCommands;
using RSBingoBot.Component_interaction_handlers;
using RSBingoBot.Leaderboard;
using static RSBingo_Framework.DAL.DataFactory;

/// <summary>
/// Class for storing code related to the long running discord bot service.
/// </summary>
public class Bot : BackgroundService
{
    private readonly ILogger logger;
    private readonly DiscordClient discordClient;
    private readonly DiscordTeam.Factory teamFactory;
    private readonly IDataWorker dataWorker = CreateDataWorker();

    /// <summary>
    /// Initializes a new instance of the <see cref="Bot"/> class.
    /// </summary>
    /// <param name="logger">The logger the instance will log to.</param>
    /// <param name="client">The client the bot will connect to.</param>
    /// <param name="teamFactory">The factory used to create instances of <see cref="Team"/>.</param>
    public Bot(ILogger<Bot> logger, DiscordClient client, DiscordTeam.Factory teamFactory)
    {
        this.logger = logger;
        this.discordClient = client;
        this.teamFactory = teamFactory;
    }

    /// <inheritdoc/>
    public override async Task StartAsync(CancellationToken stoppingToken)
    {
        discordClient.UseInteractivity();

        CommandController.RegisterSlashCommands(discordClient);

        ComponentInteractionHandler.Register<CreateTeamButtonHandler>(CreateTeamButtonHandler.CreateTeamButtonId);
        ComponentInteractionHandler.Register<JoinTeamButtonHandler>(JoinTeamButtonHandler.JoinTeamButtonId);

        await discordClient.ConnectAsync();
        await CreateExistingTeams();
        await LeaderboardDiscord.SetUp();
    }

    /// <inheritdoc/>
    public override async Task StopAsync(CancellationToken stoppingToken) =>
        await discordClient.DisconnectAsync();

    /// <inheritdoc/>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => Task.CompletedTask;

    private async Task CreateExistingTeams()
    {
        foreach (Team team in dataWorker.Teams.GetTeams())
        {
            DiscordTeam discordTeam = new (discordClient, team.Name);
            await discordTeam.InitialiseAsync(team);
        }
    }
}