// <copyright file="SendRequestResultResponsesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;

internal class SendRequestResultResponsesHandler : RequestHandler<SendRequestResultResponsesRequest>
{
    protected override async Task Process(SendRequestResultResponsesRequest request, CancellationToken cancellationToken)
    {
        if (request.Response is InteractionMessage)
        {
            var services = GetRequestService<IDiscordInteractionMessagingServices>();
            await services.Send((InteractionMessage)request.Response);
        }
        else
        {
            var services = GetRequestService<IDiscordMessageServices>();
            await services.Send(request.Response);
        }
        AddSuccess(new SendRequestResultResponsesSuccess());
    }
}