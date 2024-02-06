// <copyright file="RenameTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using static DiscordTeamChannelsInfo;

internal class RenameTeamHandler : RequestHandler<RenameTeamRequest>
{
    private const string TeamSuccessfullyRenamed = "The team '{0}' has been renamed to '{1}'.";

    private readonly IDiscordServices discordServices;
    private readonly IDatabaseServices databaseServices;
    private readonly DiscordTeamChannelsInfo channelsInfo;

    public RenameTeamHandler(IDiscordServices discordServices, IDatabaseServices databaseServices, DiscordTeamChannelsInfo channelsInfo)
    {
        this.discordServices = discordServices;
        this.databaseServices = databaseServices;
        this.channelsInfo = channelsInfo;
    }

    protected override async Task Process(RenameTeamRequest request, CancellationToken cancellationToken)
    {
        string oldName = request.DiscordTeam.Team.Name;

        if (await UpdateDatabase(request) is false)
        {
            AddError(new RenameTeamDatabaseError());
            return;
        }

        // TODO: JR - rename with the board.
        //BoardImage.RenameTeam(oldName, newName);

        await discordServices.RenameRole(request.DiscordTeam.Role!, request.DiscordTeam.RoleName);
        await RenameChannels(request);

        AddSuccess(new RenameTeamSuccess(oldName, request.NewName));
    }

    private async Task RenameChannels(RenameTeamRequest request)
    {
        await RenameChannel(request.DiscordTeam.CategoryChannel!, request, Channel.Category);
        await RenameChannel(request.DiscordTeam.BoardChannel!, request, Channel.Board);
        await RenameChannel(request.DiscordTeam.GeneralChannel!, request, Channel.General);
        await RenameChannel(request.DiscordTeam.EvidenceChannel!, request, Channel.Evidence);
        await RenameChannel(request.DiscordTeam.VoiceChannel!, request, Channel.Voice);
    }

    private async Task RenameChannel(DiscordChannel channel, RenameTeamRequest request, Channel channelType)
    {
        string newName = channelsInfo.GetInfo(request.DiscordTeam, channelType).Name;
        Result result = await discordServices.RenameChannel(channel, newName);
        if (result.IsFailed)
        {
            AddWarning(new RenameTeamChannelWarning(channelType.ToString().ToLower()));
        }
    }

    private async Task<bool> UpdateDatabase(RenameTeamRequest request)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        request.DiscordTeam.Team.Name = request.NewName;

        Result result = await databaseServices.Update(dataWorker);
        return result.IsSuccess;
    }
}