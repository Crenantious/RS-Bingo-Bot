// <copyright file="DiscordInteractionMessagingServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
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
    /// Edits the Discord message to update it to the new contents of <paramref name="message"/>.
    /// </summary>
    /// <returns>If the message was updated successfully.</returns>
    public async Task<bool> Update(InteractionMessage message) =>
        message.FollowupMessageId != 0 ?
            await EditFollowup(message.Interaction, message.GetWebhookBuilder(), message.FollowupMessageId) :
            await EditOriginalResponse(message.Interaction, message.GetWebhookBuilder());

    /// <summary>
    /// Deletes the Discord message.
    /// </summary>
    /// <returns>If the message was deleted successfully.</returns>
    public async Task<bool> Delete(InteractionMessage message)
    {
        bool succeeded = message.FollowupMessageId != 0 ?
               await DeleteFollowup(message.Interaction, message.FollowupMessageId) :
               await DeleteOriginalResponse(message.Interaction);

        if (succeeded)
        {
            MessageTagTracker.Remove(message);
        }
        return succeeded;
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

    private async Task<bool> EditOriginalResponse(DiscordInteraction interaction, DiscordWebhookBuilder builder) =>
        await SendRequest(interaction, async () => await interaction.EditOriginalResponseAsync(builder), RequestType.Edit);

    private async Task<bool> EditFollowup(DiscordInteraction interaction, DiscordWebhookBuilder builder, ulong id) =>
       await SendRequest(interaction, async () => await interaction.EditFollowupMessageAsync(id, builder), RequestType.Edit);

    private async Task<bool> DeleteOriginalResponse(DiscordInteraction interaction) =>
        await SendRequest(interaction, async () => await interaction.DeleteOriginalResponseAsync(), RequestType.Delete);

    private async Task<bool> DeleteFollowup(DiscordInteraction interaction, ulong messageId) =>
        await SendRequest(interaction, async () => await interaction.DeleteFollowupMessageAsync(messageId), RequestType.Delete);

    private async Task<bool> SendRequest(DiscordInteraction interaction, Func<Task> request, RequestType requestType)
    {
        try
        {
            await request();
            return true;
        }
        catch
        {
            return false;
        }
    }
}