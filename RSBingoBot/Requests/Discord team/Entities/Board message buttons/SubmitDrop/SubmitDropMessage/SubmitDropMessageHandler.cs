// <copyright file="SubmitDropMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.RequestHandlers;

internal class SubmitDropMessageHandler : MessageCreatedHandler<SubmitDropMessageRequest>
{
    private readonly IDiscordMessageServices messageServices;

    public SubmitDropMessageHandler(IDiscordMessageServices messageServices)
    {
        this.messageServices = messageServices;
    }

    protected override async Task Process(SubmitDropMessageRequest request, CancellationToken cancellationToken)
    {
        request.DTO.EvidenceUrl = request.MessageArgs.Message.Attachments.ElementAt(0).Url;
        request.Message.Content = request.DTO.GetMessageContent();
        await messageServices.Delete(request.MessageArgs.Message);
    }
}