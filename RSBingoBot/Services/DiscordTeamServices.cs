// <copyright file="DiscordTeamServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DSharpPlus.Entities;
using FluentResults;
using Microsoft.Extensions.Logging;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

public class DiscordTeamServices : IDiscordTeamServices
{
    private readonly Logger<DiscordTeamServices> logger;

    private DiscordTeamServices(Logger<DiscordTeamServices> logger)
    {
        this.logger = logger;
    }

    public async Task<Result<DiscordRole>> CreateTeamRole(Team team) =>
        await RequestServices.Run<CreateTeamRoleRequest, DiscordRole>(new CreateTeamRoleRequest(team));

    public async Task<Result<DiscordChannel>> CreateCategoryChannel(Team team, DiscordRole teamRole) =>
        await RequestServices.Run(new CreateTeamCategoryChannelRequest(team, teamRole));

    public async Task<Result<DiscordChannel>> CreateBoardChannel(Team team, DiscordChannel category, DiscordRole teamRole) =>
        await RequestServices.Run(new CreateTeamBoardChannelRequest(team, category, teamRole));

    public async Task<Result<DiscordChannel>> CreateGeneralChannel(Team team, DiscordChannel category, DiscordRole teamRole) =>
        await RequestServices.Run(new CreateTeamGeneralChannelRequest(team, category, teamRole));

    public async Task<Result<DiscordChannel>> CreateEvidenceChannel(Team team, DiscordChannel category, DiscordRole teamRole) =>
        await RequestServices.Run(new CreateTeamEvidenceChannelRequest(team, category, teamRole));

    public async Task<Result<DiscordChannel>> CreateVoiceChannel(Team team, DiscordChannel category, DiscordRole teamRole) =>
        await RequestServices.Run(new CreateTeamVoiceChannelRequest(team, category, teamRole));

    public async Task InitialiseBoardChannel(Team team, DiscordChannel boardChannel) =>
        await RequestServices.Run(new InitialiseTeamBoardChannelRequest(team, boardChannel));
}