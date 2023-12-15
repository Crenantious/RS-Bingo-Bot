// <copyright file="DeleteTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;

internal class DeleteTeamHandler : RequestHandler<DeleteTeamRequest>
{
    private const string TeamSuccessfullyDeletedMessage = "Team '{0}' deleted successfully.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly IDiscordServices discordServices;
    private readonly IDatabaseServices databaseServices;

    public DeleteTeamHandler(IDiscordServices discordServices, IDatabaseServices databaseServices)
    {
        this.discordServices = discordServices;
        this.databaseServices = databaseServices;
    }

    protected override async Task Process(DeleteTeamRequest request, CancellationToken cancellationToken)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();

        await DeleteRole(request);
        await DeleteChannels(request);

        // TODO: JR - handle deleting of the DiscordTeam.
        dataWorker.Teams.Remove(request.DiscordTeam.Team);
        await databaseServices.Update(dataWorker);
    }

    private async Task DeleteRole(DeleteTeamRequest request)
    {
        if (request.DiscordTeam.Role is not null)
        {
            await discordServices.DeleteRole(request.DiscordTeam.Role);
        }
    }

    private async Task DeleteChannels(DeleteTeamRequest request)
    {
        await DeleteChannel(request.DiscordTeam.CategoryChannel);
        await DeleteChannel(request.DiscordTeam.BoardChannel);
        await DeleteChannel(request.DiscordTeam.GeneralChannel);
        await DeleteChannel(request.DiscordTeam.EvidenceChannel);
        await DeleteChannel(request.DiscordTeam.VoiceChannel);
    }

    private async Task DeleteChannel(DiscordChannel? channel)
    {
        if (channel is not null)
        {
            await discordServices.DeleteChannel(channel);
        }
    }
}