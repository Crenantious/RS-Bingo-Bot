// <copyright file="GetMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;

internal class GetMessageHandler : RequestHandler<GetMessageRequest, Message>
{
    protected override async Task<Message> Process(GetMessageRequest request, CancellationToken cancellationToken)
    {
        DiscordMessage discordMessage = await request.Channel.GetMessageAsync(request.Id);
        Message message = new(discordMessage);
        AddSuccess(new GetMessageSuccess(message, request.Channel));
        return message;
    }
}