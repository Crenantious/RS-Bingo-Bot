// <copyright file="RequestRenameTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Imaging;
using RSBingo_Framework.Models;
using DSharpPlus.Entities;
using static RSBingoBot.DiscordTeam;

/// <summary>
/// Request for renaming a team.
/// </summary>
internal class RequestRenameTeam : RequestBase
{
    private const string TeamDoesNotExistError = "A team with the name '{0}' does not exist.";
    private const string TeamSuccessfullyRenamed = "The team has been renamed to '{0}'.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly string currentName;
    private readonly string newName;
    private readonly Team? team;
    private readonly DiscordInteraction interaction;

    public RequestRenameTeam(DiscordInteraction interaction, string currentName, string newName) : base(semaphore)
    {
        this.interaction = interaction;
        this.currentName = currentName;
        this.newName = newName;
        team = DataWorker.Teams.GetByName(currentName);
    }

    protected override bool Validate()
    {
        if (team is null) { AddResponse(TeamDoesNotExistError.FormatConst(currentName)); }
        else { AddResponses(RequestsUtilities.ValidateNewTeamName(newName, DataWorker).Responses); }
        return Responses.Any() is false;
    }

    protected async override Task Process()
    {
        BoardImage.RenameTeam(team!.Name, newName);
        team.Name = newName;
        await GetInstance(team).Rename(newName);
    }
}