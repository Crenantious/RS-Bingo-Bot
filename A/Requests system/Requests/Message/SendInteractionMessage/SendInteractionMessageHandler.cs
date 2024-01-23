// <copyright file="SendInteractionMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

internal class SendInteractionMessageHandler : DiscordHandler<SendInteractionMessageRequest>
{
    protected override async Task Process(SendInteractionMessageRequest request, CancellationToken cancellationToken)
    {
        if (await SendOriginalResponse(request.Message))
        {
            await OriginalResponsePostProcess(request.Message);
        }
        else
        {
            DiscordMessage discordMessage = await SendFollowupMessage(request);
            FollowupMessagePostProcess(request, discordMessage);
        }
    }

    private async Task<bool> SendOriginalResponse(InteractionMessage message) =>
        await BadRequestCheck(InteractionRespondedToCode, () =>
        {
            return message.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource,
                message.GetInteractionResponseBuilder());
        });

    private async Task OriginalResponsePostProcess(InteractionMessage message)
    {
        DiscordMessage? discordMessage = await GetOriginalResponse(message);
        if (discordMessage is null)
        {
            AddSuccess(new SendInteractionMessageWarning("response", message));
            return;
        }
        message.DiscordMessage = discordMessage;
        AddSuccess(new SendInteractionMessageSuccess("response", message));
    }

    private static async Task<DiscordMessage?> GetOriginalResponse(InteractionMessage message)
    {
        try
        {
            return await message.Interaction.GetOriginalResponseAsync();
        }
        catch
        {
            return null;
        }
    }

    private static async Task<DiscordMessage> SendFollowupMessage(SendInteractionMessageRequest request)
    {
        return await request.Message.Interaction.CreateFollowupMessageAsync(
            request.Message.GetFollowupMessageBuilder());
    }

    private void FollowupMessagePostProcess(SendInteractionMessageRequest request, DiscordMessage discordMessage)
    {
        request.Message.DiscordMessage = discordMessage;
        AddSuccess(new SendInteractionMessageSuccess("followup", request.Message));
    }
}