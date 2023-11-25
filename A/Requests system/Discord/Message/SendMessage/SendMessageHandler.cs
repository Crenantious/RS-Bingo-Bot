// <copyright file="SendMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

internal class SendMessageHandler : RequestHandler<SendMessageRequest>
{
    protected override async Task Process(SendMessageRequest request, CancellationToken cancellationToken)
    {
        await request.Channel.SendMessageAsync(request.Message.GetMessageBuilder());
        AddSuccess(new SendMessageSuccess(request.Message, request.Channel));
    }
}