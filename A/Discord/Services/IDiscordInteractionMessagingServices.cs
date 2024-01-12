// <copyright file="IDiscordInteractionMessagingServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using FluentResults;

public interface IDiscordInteractionMessagingServices : IRequestService
{
    public Task<Result> Send(Modal modal);
    public Task<Result> Send(InteractionMessage message);
    public Task<bool> Update(InteractionMessage message);
    public Task<bool> Delete(InteractionMessage message);
}