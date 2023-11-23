// <copyright file="DiscordInteractionMessagingServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;

public class DiscordInteractionMessagingServices : IDiscordInteractionMessagingServices
{
    private static readonly HashSet<ulong> InteractionsRespondedTo = new();

    private readonly Logger<DiscordInteractionMessagingServices> logger;

    private enum RequestType
    {
        Send,
        Edit,
        Delete
    }

    private DiscordInteractionMessagingServices(Logger<DiscordInteractionMessagingServices> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Sends a message to Discord in response to an interaction.
    /// </summary>
    /// <returns>If the message was sent successfully.</returns>
    public async Task<bool> Send(InteractionMessage message)
    {
        if (InteractionsRespondedTo.Contains(message.Interaction.Id))
        {
            return await Followup(message.Interaction, message.GetFollowupMessageBuilder());
        }

        bool succeeded = await CreateOriginalResponse(message.Interaction, message.GetInteractionResponseBuilder());
        if (succeeded)
        {
            InteractionsRespondedTo.Add(message.Interaction.Id);
        }

        return succeeded;
    }

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
    public async Task<bool> Delete(InteractionMessage message) =>
        message.FollowupMessageId != 0 ?
            await DeleteFollowup(message.Interaction, message.FollowupMessageId) :
            await DeleteOriginalResponse(message.Interaction);

    private async Task<bool> CreateOriginalResponse(DiscordInteraction interaction, DiscordInteractionResponseBuilder builder) =>
        await SendRequest(interaction,
            async () => await interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder),
            RequestType.Send);

    private async Task<bool> Followup(DiscordInteraction interaction, DiscordFollowupMessageBuilder builder) =>
        await SendRequest(interaction, async () => await interaction.CreateFollowupMessageAsync(builder), RequestType.Send);

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
            Log(interaction, requestType, true);
            return true;
        }
        catch
        {
            Log(interaction, requestType, false);
            return false;
        }
    }

    private void Log(DiscordInteraction interaction, RequestType requestType, bool wasSuccessful)
    {
        // TODO: JR - check if this information is sufficient.
        string outcome = wasSuccessful ? "successfully" : "unsuccessfully";
        string information = $"Interaction response was {requestType} {outcome}. " +
                             $"Id: {interaction.Id}, type: {interaction.Type}, name: {interaction.Data.Name}.";

        if (wasSuccessful)
        {
            logger.LogInformation(information);
            return;
        }

        logger.LogError(information);
    }
}