// <copyright file="DiscordInteractionMessagingServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;
using FluentResults;

public class DiscordInteractionMessagingServices : RequestService, IDiscordInteractionMessagingServices
{
    private enum RequestType
    {
        Send,
        Edit,
        Delete
    }

    /// <summary>
    /// Sends a modal to Discord in response to an interaction. The interaction must not have been responded to already.
    /// </summary>
    /// <returns>If the message was sent successfully.</returns>
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
    /// <returns>If the message was sent successfully.</returns>
    public async Task<Result> Send(InteractionMessage message) =>
        await Send(message, new SendInteractionMessageRequest(message));

    /// <summary>
    /// Deletes the Discord message.
    /// </summary>
    /// <returns>If the message was deleted successfully.</returns>
    public async Task<Result> Delete(InteractionMessage message)
    {
        var result = await RunRequest(new DeleteInteractionMessageRequest(message));

        if (result.IsSuccess && message is not Modal)
        {
            MessageTagTracker.Remove(message);
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