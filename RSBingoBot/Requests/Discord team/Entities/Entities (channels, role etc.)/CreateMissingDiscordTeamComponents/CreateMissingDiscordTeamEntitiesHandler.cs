// <copyright file="CreateMissingDiscordTeamEntitiesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using static DiscordTeamChannelsInfo;

internal class CreateMissingDiscordTeamEntitiesHandler : RequestHandler<CreateMissingDiscordTeamEntitiesRequest>
{
    private readonly DiscordTeamChannelsInfo channelsInfo;

    private IDiscordTeamServices teamServices = null!;
    private IDiscordServices discordServices = null!;
    private IDiscordMessageServices messageServices = null!;

    private RSBingoBot.Discord.DiscordTeam team = null!;

    public CreateMissingDiscordTeamEntitiesHandler(DiscordTeamChannelsInfo channelsInfo)
    {
        this.channelsInfo = channelsInfo;
    }

    protected override async Task Process(CreateMissingDiscordTeamEntitiesRequest request, CancellationToken cancellationToken)
    {

        this.team = request.DiscordTeam;
        teamServices = GetRequestService<IDiscordTeamServices>();
        discordServices = GetRequestService<IDiscordServices>();
        messageServices = GetRequestService<IDiscordMessageServices>();

        if (team.Role is null && await CreateRole() is false)
        {
            AddError(new CreateMissingDiscordTeamEntitiesRoleError());
            return;
        }

        if (team.CategoryChannel is null && await CreateChannel(request, Channel.Category, c => team.SetCategoryChannel(c)) is false)
        {
            AddError(new CreateMissingDiscordTeamEntitiesCategoryError());
            return;
        }

        await CreateChannel(request, Channel.Board, c => team.SetBoardChannel(c));
        await CreateChannel(request, Channel.General, c => team.SetGeneralChannel(c));
        await CreateChannel(request, Channel.Evidence, c => team.SetEvidenceChannel(c));
        await CreateChannel(request, Channel.Voice, c => team.SetVoiceChannel(c));
        await CreateBoardChannelMessage();

        AddSuccess(new CreateMissingDiscordTeamEntitiesSuccess());
    }

    private async Task<bool> CreateRole()
    {
        Result<DiscordRole> role = await teamServices.CreateTeamRole(team);
        if (role.IsFailed)
        {
            return false;
        }
        team.SetRole(role.Value);
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
        Result<Message> message = await teamServices.CreateBoardMessage(team);

        if (message.IsSuccess)
        {
            await messageServices.Send(message.Value, team.BoardChannel!);
            team.SetBoardMessage(message.Value);
        }
    }
}