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
using DiscordTeam = RSBingoBot.Discord.DiscordTeam;

public class DiscordTeamServices : RequestService, IDiscordTeamServices
{
    #region Management
    public async Task<Result<DiscordTeam>> CreateNewTeam(string name, IDataWorker dataWorker) =>
        await RunRequest<CreateNewTeamRequest, DiscordTeam>(new CreateNewTeamRequest(name, dataWorker));
    public async Task<Result<DiscordTeam>> CreateExistingTeam(Team team) =>
        await RunRequest<CreateExistingTeamRequest, DiscordTeam>(new CreateExistingTeamRequest(team));
    public async Task<Result> Delete(DiscordTeam discordTeam) =>
        await RunRequest(new DeleteTeamRequest(discordTeam));
    #endregion

    #region Existing entities
    public async Task<Result> CreateMissingEntities(DiscordTeam discordTeam, IDataWorker dataWorker) =>
        await RunRequest(new CreateMissingDiscordTeamEntitiesRequest(discordTeam, dataWorker));

    public async Task<Result> SetExistingEntities(DiscordTeam discordTeam) =>
        await RunRequest(new SetDiscordTeamExistingEntitiesRequest(discordTeam));
    #endregion

    #region Board message
    public async Task<Result<Message>> CreateBoardMessage(DiscordTeam discordTeam, Team team) =>
        await RunRequest<CreateTeamBoardMessageRequest, Message>(new CreateTeamBoardMessageRequest(discordTeam, team));

    public async Task<Result> UpdateBoardMessageButtons(Message message) =>
        await RunRequest(new UpdateTeamBoardMessageButtonsRequest(message));

    public async Task<Result> AddBoardToMessage(DiscordTeam discordTeam, Message message) =>
        await RunRequest(new AddTeamBoardToMessageRequest(discordTeam, message));
    #endregion

    #region User
    public async Task<Result> AddUserToTeam(DiscordUser user, DiscordTeam discordTeam, IDataWorker dataWorker) =>
        await RunRequest(new AddUserToTeamRequest(user, discordTeam, dataWorker));

    public async Task<Result> RemoveUserFromTeam(IDataWorker dataWorker, DiscordMember member, User user, DiscordTeam discordTeam) =>
        await RunRequest(new RemoveUserFromTeamRequest(dataWorker, member, user, discordTeam));
    #endregion

    public async Task<Result<DiscordRole>> CreateTeamRole(DiscordTeam discordTeam) =>
        await RunRequest<CreateTeamRoleRequest, DiscordRole>(new CreateTeamRoleRequest(discordTeam));
}