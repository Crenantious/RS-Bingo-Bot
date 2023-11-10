// <copyright file="CreateTeamRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DSharpPlus.Entities;

/// <summary>
/// Request for creating a team.
/// </summary>
internal class CreateTeamRequest : RequestBase
{
    private const string TeamSuccessfullyCreatedMessage = "The team '{0}' has been created successfully.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly string teamName;
    private readonly DiscordInteraction interaction;

    public CreateTeamRequest(DiscordInteraction interaction, string teamName) : base(semaphore)
    {
        this.interaction = interaction;
        this.teamName = teamName;
    }

    protected override async Task Process()
    {
        throw new NotImplementedException();
    }
}