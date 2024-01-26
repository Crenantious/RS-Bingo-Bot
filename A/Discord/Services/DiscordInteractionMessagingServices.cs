// <copyright file="DiscordInteractionMessagingServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using FluentResults;

public class DiscordInteractionMessagingServices : RequestService, IDiscordInteractionMessagingServices
{
    /// <summary>
    /// Sends a modal to Discord in response to an interaction. The interaction must not have been responded to already.
    /// </summary>
    public async Task<Result> Send(Modal modal, IModalRequest request)
    {
        var result = await Send(modal, new SendModalRequest(modal));
        if (result.IsSuccess)
        {
            modal.Register(request);
        }
        return result;
    }

    /// <summary>
    /// Sends a message to Discord in response to an interaction.
    /// </summary>
    public async Task<Result> Send(InteractionMessage message) =>
        await Send(message, new SendInteractionMessageRequest(message));

    /// <summary>
    /// Sends a message to Discord in response to an interaction.
    /// </summary>
    public async Task<Result> SendOriginalResponse(InteractionResponseType responseType, InteractionMessage message) =>
        await Send(message, new SendInteractionOriginalResponseRequest(responseType, message));

    /// <summary>
    /// Sends a message to Discord in response to an interaction.
    /// </summary>
    public async Task<Result> SendFollowUp(InteractionMessage message) =>
        await Send(message, new SendInteractionFollowUpRequest(message));

    /// <summary>
    /// Sends an empty message with a "thinking state" to Discord in response to an interaction.
    /// </summary>
    public async Task<Result> SendKeepAlive(DiscordInteraction interaction) =>
        await RunRequest(new SendKeepAliveInteractionMessageRequest(interaction));

    /// <summary>
    /// Deletes the Discord message.
    /// </summary>
    public async Task<Result> Delete(InteractionMessage message)
    {
        var result = await RunRequest(new DeleteInteractionMessageRequest(message));

        if (result.IsSuccess && message is not Modal)
        {
            MessageTagTracker.Add(message);
        }

        return result;
    }

    public async Task<Result<InteractionMessage>> DeleteOriginalResponse(DiscordInteraction interaction)
    {
        var result = await RunRequest<DeleteOriginalInteractionMessageRequest, InteractionMessage>(
            new DeleteOriginalInteractionMessageRequest(interaction));

        if (result.IsSuccess && result.Value is not Modal)
        {
            MessageTagTracker.Remove(result.Value);
        }

        return result;
    }

    private async Task<Result> Send(InteractionMessage message, IDiscordRequest request)
    {
        var result = await RunRequest(request);

        if (result.IsSuccess && message is not Modal)
        {
            MessageTagTracker.Add(message);
        }

        return result;
    }
}