// <copyright file="IDiscordMessageServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;

public interface IDiscordMessageServices : IRequestService
{
    public Task<Result> Send(Message message);
    public Task<Result<Message>> Get(ulong id, DiscordChannel channel);
    public Task<Result> Update(IMessage message);
    public Task<Result> Delete(Message message);
    public Task<Result> Delete(DiscordMessage message);
    public void RegisterMessageCreatedHandler(Func<IMessageCreatedRequest> getRequest, MessageCreatedDEH.Constraints constraints);
}