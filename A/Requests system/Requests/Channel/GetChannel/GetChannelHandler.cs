// <copyright file="GetChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class GetChannelHandler : RequestHandler<GetChannelRequest, DiscordChannel>
{
    protected override async Task<DiscordChannel> Process(GetChannelRequest request, CancellationToken cancellationToken)
    {
        DiscordChannel channel = DataFactory.Guild.GetChannel(request.Id);
        if (channel is null)
        {
            AddError(new GetChannelError(request.Id));
        }
        else
        {
            AddSuccess(new GetChannelSuccess(channel));
        }
        return channel!;
    }
}