// <copyright file="IDiscordInteractionMessagingServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using FluentResults;

public interface IDiscordInteractionMessagingServices : IRequestService
{
    public Task<Result> Send(Modal modal, IModalRequest request);
    public Task<Result> Send(InteractionMessage message);
    public Task<Result> SendOriginalResponse(InteractionResponseType responseType, InteractionMessage message);
    public Task<Result> SendFollowUp(InteractionMessage message);
    public Task<Result> SendKeepAlive(DiscordInteraction interaction);
    public Task<Result> Delete(InteractionMessage message);
    public Task<Result<InteractionMessage>> DeleteOriginalResponse(DiscordInteraction interaction);
}