// <copyright file="IDiscordTeamServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;

public interface IDiscordTeamServices : IRequestService
{
    public Task<Result<DiscordRole>> CreateTeamRole(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result<Message>> CreateBoardMessage(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result> SetExistingEntities(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result> CreateMissingEntities(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result> AddUserToTeam(DiscordUser discordUser, RSBingoBot.Discord.DiscordTeam team);
    public Task<Result> Delete(RSBingoBot.Discord.DiscordTeam team);
}