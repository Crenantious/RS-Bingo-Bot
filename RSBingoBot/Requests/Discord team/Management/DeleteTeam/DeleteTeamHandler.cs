// <copyright file="DeleteTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Discord;

internal class DeleteTeamHandler : RequestHandler<DeleteTeamRequest>
{
    private readonly DiscordTeamBoardButtons buttons;

    private IDiscordServices discordServices = null!;
    private IDatabaseServices databaseServices = null!;

    public DeleteTeamHandler(DiscordTeamBoardButtons buttons)
    {
        this.buttons = buttons;
    }

    protected override async Task Process(DeleteTeamRequest request, CancellationToken cancellationToken)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        discordServices = GetRequestService<IDiscordServices>();
        databaseServices = GetRequestService<IDatabaseServices>();

        await DeleteRole(request);
        await DeleteChannels(request);
        UnsubscribeBoardButtons(buttons);

        RSBingoBot.Discord.DiscordTeam.ExistingTeams.Remove(request.DiscordTeam.Name);
        dataWorker.Teams.Remove(dataWorker.Teams.Find(request.DiscordTeam.Id)!);
        await databaseServices.SaveChanges(dataWorker);

        AddSuccess(new DeleteTeamSuccess(request.DiscordTeam));
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

    private void UnsubscribeBoardButtons(DiscordTeamBoardButtons buttons)
    {
        buttons.ChangeTile.Unregister();
        buttons.SubmitEvidence.Unregister();
        buttons.SubmitDrop.Unregister();
        buttons.ViewEvidence.Unregister();
    }
}