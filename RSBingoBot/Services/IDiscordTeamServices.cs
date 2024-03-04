// <copyright file="IDiscordTeamServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using DiscordTeam = RSBingoBot.Discord.DiscordTeam;

public interface IDiscordTeamServices : IRequestService
{
    public Task<Result<DiscordTeam>> CreateNewTeam(string name, IDataWorker dataWorker);
    public Task<Result<DiscordTeam>> CreateExistingTeam(Team team);
    public Task<Result> CreateMissingEntities(DiscordTeam discordTeam, IDataWorker DataWorker);
    public Task<Result> SetExistingEntities(DiscordTeam discordTeam);
    public Task<Result<DiscordRole>> CreateTeamRole(DiscordTeam discordTeam);
    public Task<Result<Message>> CreateBoardMessage(DiscordTeam discordTeam, Team team);
    public Task<Result> AddBoardToMessage(DiscordTeam discordTeam, Message message);
    public Task<Result> AddUserToTeam(DiscordUser discordUser, DiscordTeam discordTeam, IDataWorker dataWorker);
    public Task<Result> RemoveUserFromTeam(IDataWorker dataWorker, DiscordMember member, User user, DiscordTeam discordTeam);
    public Task<Result> Delete(DiscordTeam discordTeam);
}