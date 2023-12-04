﻿// <copyright file="DiscordTeamServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;
using Microsoft.Extensions.Logging;
using RSBingoBot.Requests;

public class DiscordTeamServices : IDiscordTeamServices
{
    private readonly Logger<DiscordTeamServices> logger;

    private DiscordTeamServices(Logger<DiscordTeamServices> logger)
    {
        this.logger = logger;
    }

    public async Task<Result<DiscordRole>> CreateTeamRole(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestServices.Run<CreateTeamRoleRequest, DiscordRole>(new CreateTeamRoleRequest(team));

    public async Task<Result<DiscordChannel>> CreateCategoryChannel(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestServices.Run<CreateTeamCategoryChannelRequest, DiscordChannel>(new CreateTeamCategoryChannelRequest(team));

    public async Task<Result<DiscordChannel>> CreateBoardChannel(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestServices.Run<CreateTeamBoardChannelRequest, DiscordChannel>(new CreateTeamBoardChannelRequest(team));

    public async Task<Result<DiscordChannel>> CreateGeneralChannel(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestServices.Run<CreateTeamGeneralChannelRequest, DiscordChannel>(new CreateTeamGeneralChannelRequest(team));

    public async Task<Result<DiscordChannel>> CreateEvidenceChannel(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestServices.Run<CreateTeamEvidenceChannelRequest, DiscordChannel>(new CreateTeamEvidenceChannelRequest(team));

    public async Task<Result<DiscordChannel>> CreateVoiceChannel(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestServices.Run<CreateTeamVoiceChannelRequest, DiscordChannel>(new CreateTeamVoiceChannelRequest(team));

    public async Task<Result<Message>> CreateBoardMessage(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestServices.Run<CreateTeamBoardMessageRequest, Message>(new CreateTeamBoardMessageRequest(team));

    public async Task<Result> SetExistingEntities(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestServices.Run(new SetDiscordTeamExistingEntitiesRequest(team));

    public async Task<Result> CreateMissingEntities(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestServices.Run(new CreateMissingDiscordTeamEntitiesRequest(team));
}