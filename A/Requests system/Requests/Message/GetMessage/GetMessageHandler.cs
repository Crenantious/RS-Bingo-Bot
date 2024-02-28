// <copyright file="GetMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

internal class GetMessageHandler : DiscordHandler<GetMessageRequest, Message>
{
    protected override async Task<Message> Process(GetMessageRequest request, CancellationToken cancellationToken)
    {
        SetExceptionMessage<NotFoundException>(new GetMessageError(request.Id, request.Channel));

        DiscordMessage discordMessage = await request.Channel.GetMessageAsync(request.Id);
        Message message = new(discordMessage);
        AddSuccess(new GetMessageSuccess(message));
        return message;
    }
}