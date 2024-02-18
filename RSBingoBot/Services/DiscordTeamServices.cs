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

    public async Task<Result> CreateMissingEntities(RSBingoBot.Discord.DiscordTeam discordTeam, IDataWorker dataWorker) =>
        await RunRequest(new CreateMissingDiscordTeamEntitiesRequest(discordTeam, dataWorker));

    public async Task<Result> SetExistingEntities(RSBingoBot.Discord.DiscordTeam discordTeam) =>
        await RunRequest(new SetDiscordTeamExistingEntitiesRequest(discordTeam));

    public async Task<Result<DiscordRole>> CreateTeamRole(RSBingoBot.Discord.DiscordTeam discordTeam) =>
        await RunRequest<CreateTeamRoleRequest, DiscordRole>(new CreateTeamRoleRequest(discordTeam));

    public async Task<Result<Message>> CreateBoardMessage(RSBingoBot.Discord.DiscordTeam discordTeam, Team team) =>
        await RunRequest<CreateTeamBoardMessageRequest, Message>(new CreateTeamBoardMessageRequest(discordTeam, team));

    public async Task<Result> AddUserToTeam(DiscordUser user, RSBingoBot.Discord.DiscordTeam discordTeam, IDataWorker dataWorker) =>
        await RunRequest(new AddUserToTeamRequest(user, discordTeam, dataWorker));

    public async Task<Result> Delete(RSBingoBot.Discord.DiscordTeam discordTeam) =>
        await RunRequest(new DeleteTeamRequest(discordTeam));
}