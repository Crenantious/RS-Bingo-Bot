// <copyright file="IDiscordTeamServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;

public interface IDiscordTeamServices
{
    public Task<Result<DiscordRole>> CreateTeamRole(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result<DiscordChannel>> CreateCategoryChannel(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result<DiscordChannel>> CreateBoardChannel(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result<DiscordChannel>> CreateGeneralChannel(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result<DiscordChannel>> CreateEvidenceChannel(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result<DiscordChannel>> CreateVoiceChannel(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result<Message>> CreateBoardMessage(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result> SetExistingEntities(RSBingoBot.Discord.DiscordTeam team);
    public Task<Result> CreateMissingEntities(RSBingoBot.Discord.DiscordTeam team);
}