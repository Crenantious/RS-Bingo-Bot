// <copyright file="SendInteractionMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordServices;
using DSharpPlus;

internal class SendInteractionMessageHandler : DiscordHandler<SendInteractionMessageRequest>
{
    protected override async Task Process(SendInteractionMessageRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordInteractionMessagingServices>();
        var originalMessageResult = await messageServices.SendOriginalResponse(InteractionResponseType.ChannelMessageWithSource, request.Message);

        if (originalMessageResult.IsSuccess)
        {
            AddSuccess(new SendInteractionMessageSuccess(request.Message));
            return;
        }

        if (originalMessageResult.HasError<SendInteractionOriginalResponseAlreadyRespondedError>())
        {
            await SendFollowupMessage(request, messageServices);
            return;
        }
    }

    private async Task SendFollowupMessage(SendInteractionMessageRequest request, IDiscordInteractionMessagingServices messageServices)
    {
        var followUpMessageResult = await messageServices.SendFollowUp(request.Message);
        if (followUpMessageResult.IsSuccess)
        {
            AddSuccess(new SendInteractionMessageSuccess(request.Message));
        }
    }
}