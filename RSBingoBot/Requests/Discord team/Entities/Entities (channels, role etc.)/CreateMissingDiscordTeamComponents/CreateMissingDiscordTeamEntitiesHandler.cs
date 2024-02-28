// <copyright file="CreateMissingDiscordTeamEntitiesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.Models;
using static DiscordTeamChannelsInfo;

internal class CreateMissingDiscordTeamEntitiesHandler : RequestHandler<CreateMissingDiscordTeamEntitiesRequest>
{
    private readonly DiscordTeamChannelsInfo channelsInfo;

    private IDiscordTeamServices teamServices = null!;
    private IDiscordServices discordServices = null!;
    private IDiscordMessageServices messageServices = null!;

    private RSBingoBot.Discord.DiscordTeam discordTeam = null!;
    private Team team = null!;

    public CreateMissingDiscordTeamEntitiesHandler(DiscordTeamChannelsInfo channelsInfo)
    {
        this.channelsInfo = channelsInfo;
    }

    protected override async Task Process(CreateMissingDiscordTeamEntitiesRequest request, CancellationToken cancellationToken)
    {
        this.discordTeam = request.DiscordTeam;
        team = request.DataWorker.Teams.Find(request.DiscordTeam.Id)!;

        teamServices = GetRequestService<IDiscordTeamServices>();
        discordServices = GetRequestService<IDiscordServices>();
        messageServices = GetRequestService<IDiscordMessageServices>();

        if (discordTeam.Role is null && await CreateRole() is false)
        {
            AddError(new CreateMissingDiscordTeamEntitiesRoleError());
            return;
        }

        if (discordTeam.CategoryChannel is null &&
            await CreateChannel(request, Channel.Category, c => discordTeam.SetCategoryChannel(c, team)) is false)
        {
            AddError(new CreateMissingDiscordTeamEntitiesCategoryError());
            return;
        }

        await CreateChannel(request, Channel.Board, c => discordTeam.SetBoardChannel(c, team));
        await CreateChannel(request, Channel.General, c => discordTeam.SetGeneralChannel(c, team));
        await CreateChannel(request, Channel.Evidence, c => discordTeam.SetEvidenceChannel(c, team));
        await CreateChannel(request, Channel.Voice, c => discordTeam.SetVoiceChannel(c, team));
        await CreateBoardChannelMessage();

        AddSuccess(new CreateMissingDiscordTeamEntitiesSuccess());
    }

    private async Task<bool> CreateRole()
    {
        Result<DiscordRole> role = await teamServices.CreateTeamRole(discordTeam);
        if (role.IsFailed)
        {
            return false;
        }
        discordTeam.SetRole(role.Value, team);
        return true;
    }

    private async Task<bool> CreateChannel(CreateMissingDiscordTeamEntitiesRequest request, Channel channelType, Action<DiscordChannel> onCreation)
    {
        ChannelInfo info = channelsInfo.GetInfo(request.DiscordTeam, channelType);
        Result<DiscordChannel> channel = await discordServices.CreateChannel(info);
        if (channel.IsSuccess)
        {
            onCreation(channel.Value);
        }
        return channel.IsSuccess;
    }

    private async Task CreateBoardChannelMessage()
    {
        Result<Message> message = await teamServices.CreateBoardMessage(discordTeam, team);

        if (message.IsSuccess)
        {
            await messageServices.Send(message.Value);
            discordTeam.SetBoardMessage(message.Value, team);
        }
    }
}