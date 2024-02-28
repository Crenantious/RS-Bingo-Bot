// <copyright file="SubmitDropMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using DSharpPlus.Entities;

internal class SubmitDropMessageHandler : MessageCreatedHandler<SubmitDropMessageRequest>
{
    protected override async Task Process(SubmitDropMessageRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();
        Message message = request.GetMessage();
        DiscordAttachment attachment = message.DiscordMessage.Attachments.ElementAt(0);

        request.DTO.EvidenceUrl = attachment.Url;

        string extension = "."+attachment.MediaType.Split("/")[1];
        string imagePath = await SaveImage(request, attachment, extension);

        request.DTO.Message.RemoveAllFiles();
        request.DTO.Message.AddFile(imagePath, $"Evidence{extension}");

        await messageServices.Delete(message);
        await messageServices.Update(request.DTO.Message);
    }

    private async Task<string> SaveImage(SubmitDropMessageRequest request, DiscordAttachment attachment, string extension)
    {
        var webServices = GetRequestService<IWebServices>();

        string imagePath = Paths.GetUserTempEvidencePath(request.User.Id, extension);
        await webServices.DownloadFile(request.DTO.EvidenceUrl!, imagePath);

        return imagePath;
    }
}