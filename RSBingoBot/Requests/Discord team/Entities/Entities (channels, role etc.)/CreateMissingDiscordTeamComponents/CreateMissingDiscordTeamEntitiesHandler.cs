// <copyright file="CreateMissingDiscordTeamEntitiesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;

internal class CreateMissingDiscordTeamEntitiesHandler : RequestHandler<CreateMissingDiscordTeamEntitiesRequest>
{
    private readonly IDiscordTeamServices teamServices;
    private readonly IDiscordServices discordServices;

    private RSBingoBot.Discord.DiscordTeam team = null!;

    public CreateMissingDiscordTeamEntitiesHandler(IDiscordTeamServices teamServices, IDiscordServices discordServices)
    {
        this.teamServices = teamServices;
        this.discordServices = discordServices;
    }

    protected override async Task Process(CreateMissingDiscordTeamEntitiesRequest request, CancellationToken cancellationToken)
    {
        this.team = request.DiscordTeam;

        if (team.Role is null && await CreateRole() is false)
        {
            AddError(new CreateMissingDiscordTeamEntitiesRoleError());
            return;
        }

        DiscordTeamChannelsInfo channelsInfo = new(team);

        if (team.CategoryChannel is null && await CreateChannel(channelsInfo.Category, c => team.SetCategoryChannel(c)) is false)
        {
            AddError(new CreateMissingDiscordTeamEntitiesCategoryError());
            return;
        }

        await CreateChannel(channelsInfo.Board, c => team.SetBoardChannel(c));
        await CreateChannel(channelsInfo.General, c => team.SetGeneralChannel(c));
        await CreateChannel(channelsInfo.Evidence, c => team.SetEvidenceChannel(c));
        await CreateChannel(channelsInfo.Voice, c => team.SetVoiceChannel(c));
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

    private async Task<bool> CreateChannel(ChannelInfo creationInfo, Action<DiscordChannel> onCreation)
    {
        Result<DiscordChannel> channel = await discordServices.CreateChannel(creationInfo);
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
            await message.Value.Send(team.BoardChannel!);
            team.SetBoardMessage(message.Value);
        }
    }
}