// <copyright file="DiscordTeamServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

public class DiscordTeamServices : RequestService, IDiscordTeamServices
{
    public async Task<Result<RSBingoBot.Discord.DiscordTeam>> CreateNewTeam(string name, IDataWorker dataWorker) =>
        await RunRequest<CreateNewTeamRequest, RSBingoBot.Discord.DiscordTeam>(new CreateNewTeamRequest(name, dataWorker));

    public async Task<Result<RSBingoBot.Discord.DiscordTeam>> CreateExistingTeam(Team team) =>
        await RunRequest<CreateExistingTeamRequest, RSBingoBot.Discord.DiscordTeam>(new CreateExistingTeamRequest(team));

    public async Task<Result> CreateMissingEntities(RSBingoBot.Discord.DiscordTeam team, IDataWorker dataWorker) =>
        await RunRequest(new CreateMissingDiscordTeamEntitiesRequest(team, dataWorker));

    public async Task<Result> SetExistingEntities(RSBingoBot.Discord.DiscordTeam team) =>
        await RunRequest(new SetDiscordTeamExistingEntitiesRequest(team));

    public async Task<Result<DiscordRole>> CreateTeamRole(RSBingoBot.Discord.DiscordTeam team) =>
        await RunRequest<CreateTeamRoleRequest, DiscordRole>(new CreateTeamRoleRequest(team));

    public async Task<Result<Message>> CreateBoardMessage(RSBingoBot.Discord.DiscordTeam team) =>
        await RunRequest<CreateTeamBoardMessageRequest, Message>(new CreateTeamBoardMessageRequest(team));

    public async Task<Result> AddUserToTeam(DiscordUser user, RSBingoBot.Discord.DiscordTeam team, IDataWorker dataWorker) =>
        await RunRequest(new AddUserToTeamRequest(user, team, dataWorker));

    public async Task<Result> Delete(RSBingoBot.Discord.DiscordTeam team) =>
        await RunRequest(new DeleteTeamRequest(team));
}