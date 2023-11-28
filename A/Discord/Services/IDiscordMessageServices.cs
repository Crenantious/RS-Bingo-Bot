// <copyright file="IDiscordMessageServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

// TODO: JR - implement. Remember to set the Message.Id when sending or getting.
public interface IDiscordMessageServices
{
    public Task<bool> Send(Message message, DiscordChannel channel);
    public Task<Message> Get(ulong id, DiscordChannel channel);
}