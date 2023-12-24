// <copyright file="DeleteChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

internal class DeleteChannelHandler : RequestHandler<DeleteChannelRequest>
{
    protected override async Task Process(DeleteChannelRequest request, CancellationToken cancellationToken)
    {
        await request.Channel.DeleteAsync();
        AddSuccess(new DeleteChannelSuccess(request.Channel));
    }
}