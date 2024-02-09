﻿// <copyright file="SendRequestResultResponsesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordServices;

internal class SendRequestResultResponsesHandler : RequestHandler<SendRequestResultResponsesRequest>
{
    protected override async Task Process(SendRequestResultResponsesRequest request, CancellationToken cancellationToken)
    {
        var services = GetRequestService<IDiscordInteractionMessagingServices>();
        await services.Send(request.Response);
        AddSuccess(new SendRequestResultResponsesSuccess());
    }
}