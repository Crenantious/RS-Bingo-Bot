// <copyright file="DeleteMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

internal class DeleteMessageHandler : DiscordHandler<DeleteMessageRequest>
{
    protected override async Task Process(DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        await request.Message.DeleteAsync();
        AddSuccess(new DeleteMessageSuccess(request.Message));
    }
}