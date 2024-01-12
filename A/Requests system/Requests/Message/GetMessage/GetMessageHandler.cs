// <copyright file="GetMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;

internal class GetMessageHandler : DiscordHandler<GetMessageRequest, Message>
{
    protected override async Task<Message> Process(GetMessageRequest request, CancellationToken cancellationToken)
    {
        DiscordMessage discordMessage = await request.Channel.GetMessageAsync(request.Id);
        Message message = new(discordMessage);
        AddSuccess(new GetMessageSuccess(message));
        return message;
    }
}