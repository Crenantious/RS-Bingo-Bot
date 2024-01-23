﻿// <copyright file="SendKeepAliveInteractionMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DSharpPlus.Entities;

namespace DiscordLibrary.Requests;

internal class SendKeepAliveInteractionMessageHandler : DiscordHandler<SendKeepAliveInteractionMessageRequest>
{
    protected override async Task Process(SendKeepAliveInteractionMessageRequest request, CancellationToken cancellationToken)
    {
        bool success = await BadRequestCheck(InteractionRespondedToCode, () => SendMessage(request));

        if (success)
        {
            AddSuccess(new SendKeepAliveInteractionMessageSuccess(request.Interaction));
        }
        else
        {
            AddError(new SendKeepAliveInteractionMessageError(request.Interaction));
        }
    }

    private static async Task SendMessage(SendKeepAliveInteractionMessageRequest request)
    {
        var builder = new DiscordInteractionResponseBuilder()
            .AsEphemeral();
        await request.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource, builder);
    }
}