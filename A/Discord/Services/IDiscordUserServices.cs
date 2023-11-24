// <copyright file="IDiscordUserServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DSharpPlus.Entities;
using FluentResults;

public interface IDiscordUserServices
{
    public Task<Result<DiscordMember>> GetUser(ulong id);
    public Task<Result> GrantRole(DiscordMember member, DiscordRole role);
}