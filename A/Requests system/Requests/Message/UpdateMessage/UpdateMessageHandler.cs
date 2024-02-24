// <copyright file="UpdateMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;

internal class UpdateMessageHandler : RequestHandler<UpdateMessageRequest>
{
    protected override async Task Process(UpdateMessageRequest request, CancellationToken cancellationToken)
    {
        DiscordMessage message = await request.Message.DiscordMessage.ModifyAsync(request.Message.GetMessageBuilder());
        request.Message.OnMessageSent(message);
        AddSuccess(new UpdateMessageSuccess(request.Message));
    }
}