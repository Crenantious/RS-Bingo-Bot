// <copyright file="GetLeaderboardMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using RSBingo_Framework.DAL;

internal class GetLeaderboardMessageHandler : RequestHandler<GetLeaderboardMessageRequest, Message>
{
    protected override async Task<Message> Process(GetLeaderboardMessageRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();

        var result = await messageServices.Get(DataFactory.LeaderboardMessageId, DataFactory.LeaderboardChannel);
        if (result.IsFailed)
        {
            AddError(new GetLeaderboardMessageError(DataFactory.LeaderboardMessageId));
            return null!;
        }

        return result.Value;
    }
}