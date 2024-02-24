// <copyright file="IDiscordTeamServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

public interface IDiscordTeamServices : IRequestService
{
    public Task<Result<RSBingoBot.Discord.DiscordTeam>> CreateNewTeam(string name, IDataWorker dataWorker);
    public Task<Result<RSBingoBot.Discord.DiscordTeam>> CreateExistingTeam(Team team);
    public Task<Result> CreateMissingEntities(RSBingoBot.Discord.DiscordTeam discordTeam, IDataWorker DataWorker);
    public Task<Result> SetExistingEntities(RSBingoBot.Discord.DiscordTeam discordTeam);
    public Task<Result<DiscordRole>> CreateTeamRole(RSBingoBot.Discord.DiscordTeam discordTeam);
    public Task<Result<Message>> CreateBoardMessage(RSBingoBot.Discord.DiscordTeam discordTeam, Team team);
    public Task<Result> AddBoardToMessage(Team team, Message message);
    public Task<Result> AddUserToTeam(DiscordUser discordUser, RSBingoBot.Discord.DiscordTeam discordTeam, IDataWorker dataWorker);
    public Task<Result> Delete(RSBingoBot.Discord.DiscordTeam discordTeam);
}