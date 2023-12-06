// <copyright file="DeleteMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

internal class DeleteMessageHandler : RequestHandler<DeleteMessageRequest>
{
    public DeleteMessageHandler()
    {

    }

    protected override async Task Process(DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        await request.Message.DiscordMessage.DeleteAsync();
        AddSuccess(new DeleteMessageSuccess(request.Message, request.Message.DiscordMessage.Channel));
    }
}