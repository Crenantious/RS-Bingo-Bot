// <copyright file="SubmitEvidenceMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using DSharpPlus.Entities;

internal class SubmitEvidenceMessageHandler : MessageCreatedHandler<SubmitEvidenceMessageRequest>
{
    protected override async Task Process(SubmitEvidenceMessageRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();
        Message message = request.GetMessage();
        DiscordAttachment attachment = message.DiscordMessage.Attachments.ElementAt(0);

        request.DTO.EvidenceUrl = attachment.Url;

        string extension = "." + attachment.MediaType.Split("/")[1];
        string path = await SaveImage(request, attachment, extension);

        request.EvidenceFile.SetContent(path);

        await messageServices.Delete(message);
        await messageServices.Update(request.DTO.Message);
    }

    private async Task<string> SaveImage(SubmitEvidenceMessageRequest request, DiscordAttachment attachment, string extension)
    {
        var webServices = GetRequestService<IWebServices>();

        string path = Paths.GetUserTempEvidencePath(request.User.Id, extension);
        await webServices.DownloadFile(request.DTO.EvidenceUrl!, path);

        return path;
    }
}