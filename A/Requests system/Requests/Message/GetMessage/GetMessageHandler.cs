// <copyright file="GetMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

internal class GetMessageHandler : DiscordHandler<GetMessageRequest, Message>
{
    private readonly MessageFactory messageFactory;

    public GetMessageHandler(MessageFactory messageFactory)
    {
        this.messageFactory = messageFactory;
    }

    protected override async Task<Message> Process(GetMessageRequest request, CancellationToken cancellationToken)
    {
        SetExceptionMessage<NotFoundException>(new GetMessageError(request.Id, request.Channel));

        var webServices = GetRequestService<IWebServices>();

        DiscordMessage discordMessage = await request.Channel.GetMessageAsync(request.Id);
        Message message = await messageFactory.Create(discordMessage, webServices);

        AddSuccess(new GetMessageSuccess(message));
        return message;
    }
}