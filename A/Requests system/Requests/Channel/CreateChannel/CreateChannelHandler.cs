// <copyright file="CreateChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class CreateChannelHandler : DiscordHandler<CreateChannelRequest, DiscordChannel>
{
    protected override async Task<DiscordChannel> Process(CreateChannelRequest request, CancellationToken cancellationToken)
    {
        DiscordChannel channel = await DataFactory.Guild.CreateChannelAsync(request.Name,
            request.ChannelType, request.Parent!, overwrites: request.Overwrites!);
        AddSuccess(new CreateChannelSuccess(channel));
        return channel;
    }
}