// <copyright file="SendMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

internal class SendMessageHandler : DiscordHandler<SendMessageRequest>
{
    protected override async Task Process(SendMessageRequest request, CancellationToken cancellationToken)
    {
        DiscordMessage message = await request.Message.Channel.SendMessageAsync(request.Message.GetMessageBuilder());
        request.Message.OnMessageSent(message);
        AddSuccess(new SendMessageSuccess(request.Message));
    }
}