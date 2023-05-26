// <copyright file="RequestCreateTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingoBot.DTO;
using RSBingoBot.Leaderboard;
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

    public RequestCreateTeam(DiscordInteraction interaction, string teamName) : base(semaphore)
    {
        this.interaction = interaction;
        this.teamName = teamName;
    }

    protected override async Task<RequestResult<string>> Validate() =>
        RequestsUtilities.ValidateNewTeamName(teamName, DataWorker);

    protected override async Task<RequestResult<string>> Process()
    {
        throw new NotImplementedException();
    }
}