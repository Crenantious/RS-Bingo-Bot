// <copyright file="SubmitDropMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.RequestHandlers;

internal class SubmitDropMessageHandler : MessageCreatedHandler<SubmitDropMessageRequest>
{
    protected override async Task Process(SubmitDropMessageRequest request, CancellationToken cancellationToken)
    {
        request.DTO.EvidenceUrl = request.MessageArgs.Message.Attachments.ElementAt(0).Url;
    }
}