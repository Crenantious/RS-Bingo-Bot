// <copyright file="DiscordTeamServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

public class DiscordTeamServices : IDiscordTeamServices
{
    public async Task<Result<DiscordRole>> CreateTeamRole(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestRunner.Run<CreateTeamRoleRequest, DiscordRole>(new CreateTeamRoleRequest(team));

    public async Task<Result<Message>> CreateBoardMessage(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestRunner.Run<CreateTeamBoardMessageRequest, Message>(new CreateTeamBoardMessageRequest(team));

    public async Task<Result> SetExistingEntities(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestRunner.Run(new SetDiscordTeamExistingEntitiesRequest(team));

    public async Task<Result> CreateMissingEntities(RSBingoBot.Discord.DiscordTeam team) =>
        await RequestRunner.Run(new CreateMissingDiscordTeamEntitiesRequest(team));

    public async Task<Result> AddUserToTeam(DiscordUser user, RSBingoBot.Discord.DiscordTeam team) =>
        await RequestRunner.Run(new AddUserToTeamRequest(user, team));
}