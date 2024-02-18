// <copyright file="SetDiscordTeamExistingEntitiesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Discord;

internal class SetDiscordTeamExistingEntitiesHandler : RequestHandler<SetDiscordTeamExistingEntitiesRequest>
{
    private readonly DiscordTeamBoardButtons boardButtons;

    private IDiscordServices discordServices = null!;
    private IDiscordMessageServices messageServices = null!;
    private Team team = null!;

    public SetDiscordTeamExistingEntitiesHandler(DiscordTeamBoardButtons boardButtons)
    {
        this.boardButtons = boardButtons;
    }

    protected override async Task Process(SetDiscordTeamExistingEntitiesRequest request, CancellationToken cancellationToken)
    {
        discordServices = GetRequestService<IDiscordServices>();
        messageServices = GetRequestService<IDiscordMessageServices>();
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        team = dataWorker.Teams.Find(request.DiscordTeam.Id)!;

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
        Result<DiscordRole> role = await discordServices.GetRole(team.RoleId);
        if (role.IsSuccess)
        {
            request.DiscordTeam.SetRole(role.Value, team);
        }
    }

    private async Task SetCategoryChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(team.CategoryChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetCategoryChannel(channel.Value, team);
        }
    }

    private async Task SetBoardChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(team.BoardChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetBoardChannel(channel.Value, team);
        }
    }

    private async Task SetGeneralChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(team.GeneralChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetGeneralChannel(channel.Value, team);
        }
    }

    private async Task SetEvidenceChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(team.EvidenceChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetEvidenceChannel(channel.Value, team);
        }
    }

    private async Task SetVoiceChannel(SetDiscordTeamExistingEntitiesRequest request)
    {
        Result<DiscordChannel> channel = await discordServices.GetChannel(team.VoiceChannelId);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetVoiceChannel(channel.Value, team);
        }
    }

    private async Task SetBoardMessage(SetDiscordTeamExistingEntitiesRequest request)
    {
        if (request.DiscordTeam.BoardChannel is null)
        {
            return;
        }

        Result<Message> message = await messageServices.Get(team.BoardMessageId, request.DiscordTeam.BoardChannel);
        if (message.IsSuccess)
        {
            request.DiscordTeam.SetBoardMessage(message.Value, team);
            boardButtons.Create(request.DiscordTeam);
        }
    }
}