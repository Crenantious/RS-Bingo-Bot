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

        if (team.CategoryChannel is null && await CreateCategoryChannel() is false)
        {
            AddError(new CreateMissingDiscordTeamEntitiesCategoryError());
            return;
        }

        await CreateBoardChannel();
        await CreateGeneralChannel();
        await CreateEvidenceChannel();
        await CreateVoiceChannel();
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

    private async Task<bool> CreateCategoryChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateCategoryChannel(team);
        if (channel.IsFailed)
        {
            return false;
        }
        team.SetCategoryChannel(channel.Value);
        return true;
    }

    private async Task<bool> CreateBoardChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateBoardChannel(team);

        if (channel.IsFailed)
        {
            return false;
        }
        team.SetBoardChannel(channel.Value);
        return true;
    }

    private async Task<bool> CreateGeneralChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateGeneralChannel(team);

        if (channel.IsFailed)
        {
            return false;
        }
        team.SetGeneralChannel(channel.Value);
        return true;
    }

    private async Task<bool> CreateEvidenceChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateEvidenceChannel(team);

        if (channel.IsFailed)
        {
            return false;
        }
        team.SetEvidenceChannel(channel.Value);
        return true;
    }

    private async Task<bool> CreateVoiceChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateVoiceChannel(team);

        if (channel.IsFailed)
        {
            return false;
        }
        team.SetVoiceChannel(channel.Value);
        return true;
    }

    private async Task<bool> CreateBoardChannelMessage()
    {
        Result<Message> message = await teamServices.CreateBoardMessage(team);

        if (message.IsFailed)
        {
            return false;
        }
        team.SetBoardMessage(message.Value);
        message.Value.Send(team.BoardChannel!);
        return true;
    }
}