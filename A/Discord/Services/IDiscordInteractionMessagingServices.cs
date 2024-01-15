// <copyright file="IDiscordInteractionMessagingServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using FluentResults;

public interface IDiscordInteractionMessagingServices : IRequestService
{
    public Task<Result> Send(Modal modal, IModalRequest request);
    public Task<Result> Send(InteractionMessage message);
    public Task<bool> Update(InteractionMessage message);
    public Task<bool> Delete(InteractionMessage message);
}