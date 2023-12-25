// <copyright file="SetDiscordTeamExistingEntitiesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;

internal class SetDiscordTeamExistingEntitiesHandler : RequestHandler<SetDiscordTeamExistingEntitiesRequest>
{
    private readonly IDiscordServices discordServices;
    private readonly IDiscordMessageServices messageServices;

    public SetDiscordTeamExistingEntitiesHandler(IDiscordServices discordServices, IDiscordMessageServices messageServices)
    {
        this.discordServices = discordServices;
        this.messageServices = messageServices;
    }

    protected override async Task Process(SetDiscordTeamExistingEntitiesRequest request, CancellationToken cancellationToken)
    {
        await SetRole(request);
        await SetCategoryChannel(request);
        await SetBoardChannel(request);
        await SetGeneralChannel(request);
        await SetEvidenceChannel(request);
        await SetVoiceChannel(request);
        await SetBoardMessage(request);
        AddSuccess(new SetDiscordTeamExistingEntitiesSuccess());
    }

    private async Task SetRole(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordRole> role = await discordServices.GetRole(request.DiscordTeam.Team.RoleId);
        if (role.IsSuccess)
        {
            request.DiscordTeam.SetRole(role.Value);
        }
    }

    private async Task SetCategoryChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(request.DiscordTeam.Team.CategoryChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetCategoryChannel(channel.Value);
        }
    }

    private async Task SetBoardChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(request.DiscordTeam.Team.BoardChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetBoardChannel(channel.Value);
        }
    }

    private async Task SetGeneralChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(request.DiscordTeam.Team.GeneralChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetGeneralChannel(channel.Value);
        }
    }

    private async Task SetEvidenceChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(request.DiscordTeam.Team.EvidenceChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetEvidenceChannel(channel.Value);
        }
    }

    private async Task SetVoiceChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(request.DiscordTeam.Team.VoiceChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetVoiceChannel(channel.Value);
        }
    }

    private async Task SetBoardMessage(SetDiscordTeamExistingEntitiesRequest request)
    {
        if (request.DiscordTeam.BoardChannel is null)
        {
            return;
        }

        Result<Message> message = await messageServices.Get(request.DiscordTeam.Team.BoardMessageId, request.DiscordTeam.BoardChannel);
        if (message.IsSuccess)
        {
            request.DiscordTeam.SetBoardMessage(message.Value);
        }
    }
}