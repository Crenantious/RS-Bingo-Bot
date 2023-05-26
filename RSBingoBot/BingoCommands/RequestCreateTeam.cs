// <copyright file="RequestCreateTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingoBot.DTO;
using RSBingoBot.Leaderboard;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using DSharpPlus.Entities;

/// <summary>
/// Request for creating a team.
/// </summary>
internal class RequestCreateTeam : RequestBase
{
    private const string TeamSuccessfullyCreatedMessage = "The team '{0}' has been created successfully.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly string teamName;
    private readonly DiscordInteraction interaction;
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public RequestCreateTeam(DiscordInteraction interaction, string teamName) : base(semaphore)
    {
        this.interaction = interaction;
        this.teamName = teamName;
    }

    protected override async Task<Result<string>> Validate() =>
        RequestsUtilities.ValidateNewTeamName(teamName, dataWorker);

    protected override async Task<Result<string>> Process()
    {
        await new RSBingoBot.DiscordTeam(Ctx.Client, teamName).InitialiseAsync();
        await LeaderboardDiscord.Update(dataWorker);
    }
}