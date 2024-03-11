// <copyright file="Bot.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using DiscordLibrary.DiscordServices;
using DSharpPlus;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Commands;
using RSBingoBot.Discord;
using RSBingoBot.DiscordComponents;
using RSBingoBot.Requests;
using static RSBingo_Framework.DAL.DataFactory;

/// <summary>
/// Class for storing code related to the long running discord bot service.
/// </summary>
internal class Bot : BackgroundService
{
    private readonly ILogger logger;
    private readonly DiscordClient discordClient;
    private readonly SingletonButtons singletonButtons;
    private readonly CommandController commandController;
    private readonly IDiscordMessageServices messageServices;
    private readonly IEvidenceVerificationEmojis evidenceVerificationEmojis;
    private readonly IScoringServices leaderboardServices;
    private readonly IDataWorker dataWorker = CreateDataWorker();

    /// <summary>
    /// Initializes a new instance of the <see cref="Bot"/> class.
    /// </summary>
    /// <param name="logger">The logger the instance will log to.</param>
    /// <param name="client">The client the bot will connect to.</param>
    /// <param name="teamFactory">The factory used to create instances of <see cref="Team"/>.</param>
    public Bot(ILogger<Bot> logger, DiscordClient client, SingletonButtons singletonButtons, CommandController commandController,
        IDiscordMessageServices messageServices, IEvidenceVerificationEmojis evidenceVerificationEmojis,
        IScoringServices leaderboardServices, LeaderboardMessage leaderboardMessage)
    {
        this.logger = logger;
        this.discordClient = client;

        // Injected with DI so this is enough for all the buttons to be created.
        // TODO: JR - make the buttons themselves injected or make SingletonButtons eager loaded so
        // injecting here is unnecessary.
        this.singletonButtons = singletonButtons;
        this.commandController = commandController;
        this.messageServices = messageServices;
        this.evidenceVerificationEmojis = evidenceVerificationEmojis;
        this.leaderboardServices = leaderboardServices;

        messageServices.Initialise(null);
        leaderboardServices.Initialise(null);
    }

    /// <inheritdoc/>
    public override async Task StartAsync(CancellationToken stoppingToken)
    {
        discordClient.UseInteractivity();

        commandController.RegisterSlashCommands(discordClient);

        await discordClient.ConnectAsync();
        await CreateExistingTeams();
        await leaderboardServices.SetUpLeaderboardMessage();
        RegisterEvidenceReactionRequests();
    }

    /// <inheritdoc/>
    public override async Task StopAsync(CancellationToken stoppingToken) =>
        await discordClient.DisconnectAsync();

    /// <inheritdoc/>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => Task.CompletedTask;

    private async Task CreateExistingTeams()
    {
        var teamServices = (IDiscordTeamServices)General.DI.GetService(typeof(IDiscordTeamServices))!;
        teamServices.Initialise(null);

        foreach (Team team in dataWorker.Teams.GetTeams())
        {
            var result = await teamServices.CreateExistingTeam(team);
            if (result.IsSuccess)
            {
                DiscordTeam.ExistingTeams.Add(team.Name, result.Value);
            }
        }
    }

    private void RegisterEvidenceReactionRequests()
    {
        messageServices.RegisterMessageReactedHandler(() =>
            new EvidenceVerificationReactionRequest(evidenceVerificationEmojis.Verified),
            args => args.Channel == DataFactory.PendingReviewEvidenceChannel);
        messageServices.RegisterMessageReactedHandler(() =>
            new EvidenceVerificationReactionRequest(evidenceVerificationEmojis.Verified),
            args => args.Channel == DataFactory.RejectedEvidenceChannel);

        messageServices.RegisterMessageReactedHandler(() =>
            new EvidenceRejectionReactionRequest(evidenceVerificationEmojis.Rejected),
            args => args.Channel == DataFactory.PendingReviewEvidenceChannel);
        messageServices.RegisterMessageReactedHandler(() =>
            new EvidenceRejectionReactionRequest(evidenceVerificationEmojis.Rejected),
            args => args.Channel == DataFactory.VerifiedEvidenceChannel);
    }
}