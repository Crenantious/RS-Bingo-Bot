// <copyright file="DeleteMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Exceptions;

internal class DeleteMessageHandler : DiscordHandler<DeleteMessageRequest>
{
    protected override async Task Process(DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        SetExceptionMessage<NotFoundException>(new DeleteMessageError(request.Message));

        await request.Message.DeleteAsync();
        AddSuccess(new DeleteMessageSuccess(request.Message));
    }
}