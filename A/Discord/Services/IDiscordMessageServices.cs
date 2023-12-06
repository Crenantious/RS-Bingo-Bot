// <copyright file="IDiscordMessageServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;

public interface IDiscordMessageServices
{
    public Task<bool> Send(Message message, DiscordChannel channel);
    public Task<Message> Get(ulong id, DiscordChannel channel);
    public void RegisterCreationHandler(IMessageCreatedRequest request, MessageCreatedDEH.Constraints constraints);
}