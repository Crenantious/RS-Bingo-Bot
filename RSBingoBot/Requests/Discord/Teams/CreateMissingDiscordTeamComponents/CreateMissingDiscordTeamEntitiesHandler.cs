// <copyright file="CreateMissingDiscordTeamEntitiesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;

internal class CreateMissingDiscordTeamEntitiesHandler : RequestHandler<CreateMissingDiscordTeamEntitiesRequest>
{
    private readonly IDiscordTeamServices teamServices;
    private readonly IDiscordServices discordServices;

    private RSBingoBot.DTO.DiscordTeam discordTeam = null!;

    public CreateMissingDiscordTeamEntitiesHandler(IDiscordTeamServices teamServices, IDiscordServices discordServices)
    {
        this.teamServices = teamServices;
        this.discordServices = discordServices;
    }

    protected override async Task Process(CreateMissingDiscordTeamEntitiesRequest request, CancellationToken cancellationToken)
    {
        this.discordTeam = request.DiscordTeam;

        if (discordTeam.Role is null && await CreateRole() is false)
        {
            return;
        }

        if (discordTeam.CategoryChannel is null && await CreateCategoryChannel() is false)
        {
            return;
        }

        await CreateGeneralChannel();
        await CreateEvidenceChannel();
        await CreateVoiceChannel();

        AddSuccess(new CreateMissingDiscordTeamEntitiesSuccess());
    }

    private async Task<bool> CreateRole()
    {
        Result<DiscordRole> role = await teamServices.CreateTeamRole(discordTeam.Team);
        if (role.IsFailed)
        {
            AddError(new CreateMissingDiscordTeamEntitiesRoleError());
            return false;
        }
        discordTeam.SetRole(role.Value);
        return true;
    }

    private async Task<bool> CreateCategoryChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateCategoryChannel(discordTeam.Team, discordTeam.Role!);
        if (channel.IsFailed)
        {
            AddError(new CreateMissingDiscordTeamEntitiesCategoryError());
            return false;
        }
        discordTeam.SetCategoryChannel(channel.Value);
        return true;
    }

    private async Task<bool> CreateBoardChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateBoardChannel(
            discordTeam.Team, discordTeam.CategoryChannel!, discordTeam.Role!);

        if (channel.IsFailed)
        {
            return false;
        }
        discordTeam.SetBoardChannel(channel.Value);
        await teamServices.InitialiseBoardChannel(discordTeam.Team, channel.Value);
        return true;
    }

    private async Task<bool> CreateGeneralChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateGeneralChannel(
            discordTeam.Team, discordTeam.CategoryChannel!, discordTeam.Role!);

        if (channel.IsFailed)
        {
            return false;
        }
        discordTeam.SetGeneralChannel(channel.Value);
        return true;
    }

    private async Task<bool> CreateEvidenceChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateEvidenceChannel(
            discordTeam.Team, discordTeam.CategoryChannel!, discordTeam.Role!);

        if (channel.IsFailed)
        {
            return false;
        }
        discordTeam.SetEvidenceChannel(channel.Value);
        return true;
    }

    private async Task<bool> CreateVoiceChannel()
    {
        Result<DiscordChannel> channel = await teamServices.CreateVoiceChannel(
            discordTeam.Team, discordTeam.CategoryChannel!, discordTeam.Role!);

        if (channel.IsFailed)
        {
            return false;
        }
        discordTeam.SetVoiceChannel(channel.Value);
        return true;
    }
}