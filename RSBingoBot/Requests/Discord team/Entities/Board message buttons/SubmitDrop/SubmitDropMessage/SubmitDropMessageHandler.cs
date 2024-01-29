// <copyright file="SubmitDropMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;

internal class SubmitDropMessageHandler : MessageCreatedHandler<SubmitDropMessageRequest>
{
    private readonly IDiscordMessageServices messageServices;

    public SubmitDropMessageHandler(IDiscordMessageServices messageServices)
    {
        this.messageServices = messageServices;
    }

    protected override async Task Process(SubmitDropMessageRequest request, CancellationToken cancellationToken)
    {
        Message message = request.GetMessage();
        request.DTO.EvidenceUrl = message.DiscordMessage.Attachments.ElementAt(0).Url;
        message.Content = request.DTO.GetMessageContent();
        await messageServices.Delete(message);
    }
}