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

internal class RenameTeamHandler : RequestHandler<RenameTeamRequest>
{
    private const string TeamSuccessfullyRenamed = "The team '{0}' has been renamed to '{1}'.";

    private readonly IDiscordServices discordServices;
    private readonly IDatabaseServices databaseServices;

    public RenameTeamHandler(IDiscordServices discordServices, IDatabaseServices databaseServices)
    {
        this.discordServices = discordServices;
        this.databaseServices = databaseServices;
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
        DiscordTeamChannelsInfo channelsInfo = new(request.DiscordTeam);
        await RenameChannel(request.DiscordTeam.CategoryChannel!, channelsInfo.Category.Name, "category");
        await RenameChannel(request.DiscordTeam.BoardChannel!, channelsInfo.Board.Name, "board");
        await RenameChannel(request.DiscordTeam.GeneralChannel!, channelsInfo.General.Name, "general");
        await RenameChannel(request.DiscordTeam.EvidenceChannel!, channelsInfo.Evidence.Name, "evidence");
        await RenameChannel(request.DiscordTeam.VoiceChannel!, channelsInfo.Voice.Name, "voice");
    }

    private async Task RenameChannel(DiscordChannel channel, string name, string typeName)
    {
        Result result = await discordServices.RenameChannel(channel, name);
        if (result.IsFailed)
        {
            AddWarning(new RenameTeamChannelWarning(typeName));
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