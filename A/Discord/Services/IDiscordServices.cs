// <copyright file="IDiscordServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DSharpPlus.Entities;
using FluentResults;

public interface IDiscordServices
{
    public Task<Result<DiscordMember>> GetUser(ulong id);
    public Task<Result<DiscordRole>> CreateRole(string name);

    public Task<Result> GrantRole(DiscordMember member, DiscordRole role);
}