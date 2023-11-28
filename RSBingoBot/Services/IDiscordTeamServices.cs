// <copyright file="IDiscordTeamServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.Models;

public interface IDiscordTeamServices
{
    public Task<Result<DiscordRole>> CreateTeamRole(Team team);
    public Task<Result<DiscordChannel>> CreateCategoryChannel(Team team, DiscordRole teamRole);
    public Task<Result<DiscordChannel>> CreateBoardChannel(Team team, DiscordChannel category, DiscordRole teamRole);
    public Task<Result<DiscordChannel>> CreateGeneralChannel(Team team, DiscordChannel category, DiscordRole teamRole);
    public Task<Result<DiscordChannel>> CreateEvidenceChannel(Team team, DiscordChannel category, DiscordRole teamRole);
    public Task<Result<DiscordChannel>> CreateVoiceChannel(Team team, DiscordChannel category, DiscordRole teamRole);
    public Task<Result> InitialiseBoardChannel(Team team, DiscordChannel boardChannel);
    public Task<Result<Message>> CreateBoardMessage(Team team, DiscordChannel boardChannel);
}