// <copyright file="SendInteractionMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;

internal class SendInteractionMessageHandler : DiscordHandler<SendInteractionMessageRequest>
{
    protected override async Task Process(SendInteractionMessageRequest request, CancellationToken cancellationToken)
    {
        if (await HasResponse(request.Message.Interaction))
        {
            await request.Message.Interaction.CreateFollowupMessageAsync(request.Message.GetFollowupMessageBuilder());
            AddSuccess(new SendInteractionMessageSuccess(request.Message, "followup"));
        }
        else
        {
            await request.Message.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource,
                request.Message.GetInteractionResponseBuilder());
            AddSuccess(new SendInteractionMessageSuccess(request.Message, "response"));
        }
    }

    private async Task<bool> HasResponse(DiscordInteraction interaction)
    {
        // TODO: JR - check the specific exception.
        try
        {
            await interaction.GetOriginalResponseAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

}