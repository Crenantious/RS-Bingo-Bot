// <copyright file="SendKeepAliveInteractionMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

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
            AddError(new SendKeepAliveInteractionMessageAlreadyRespondedError(request.Interaction));
        }
    }

    private static async Task SendMessage(SendKeepAliveInteractionMessageRequest request)
    {
        var builder = new DiscordInteractionResponseBuilder();
        builder.IsEphemeral = request.IsEphemeral;
        await request.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredChannelMessageWithSource, builder);
    }
}