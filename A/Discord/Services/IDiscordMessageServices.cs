// <copyright file="IDiscordMessageServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

public interface IDiscordMessageServices
{
    public Task<bool> Send(Message message, DiscordChannel channel);
}