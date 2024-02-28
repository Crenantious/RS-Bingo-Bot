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
    protected override async Task Process(SubmitDropMessageRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();

        Message message = request.GetMessage();
        request.DTO.EvidenceUrl = message.DiscordMessage.Attachments.ElementAt(0).Url;
        message.Content = request.DTO.GetMessageContent();
        await messageServices.Delete(message);
    }
}