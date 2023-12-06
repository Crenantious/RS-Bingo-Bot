// <copyright file="SendMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;

internal class SendMessageHandler : RequestHandler<SendMessageRequest>
{
    protected override async Task Process(SendMessageRequest request, CancellationToken cancellationToken)
    {
        DiscordMessage message = await request.Channel.SendMessageAsync(request.Message.GetMessageBuilder());
        request.Message.DiscordMessage = message;
        AddSuccess(new SendMessageSuccess(request.Message, request.Channel));
    }
}