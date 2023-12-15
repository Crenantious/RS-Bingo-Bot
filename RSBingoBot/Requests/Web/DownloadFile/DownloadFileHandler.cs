// <copyright file="DownloadFileHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingoBot.Web;
using System.Net;

internal class DownloadFileHandler : RequestHandler<DownloadFileRequest>
{
    protected override async Task Process(DownloadFileRequest request, CancellationToken cancellationToken)
    {
        WebClient webClient = new();

        // TODO: JR - check errors.
        try
        {
            if (WhitelistChecker.IsUrlWhitelisted(request.Url) is false)
            {
                AddError(new DownloadFileDomainError());
                return;
            }
            webClient.DownloadFile(request.Url, request.Path);
        }
        catch (WebException e)
        {
            AddError(new DownloadFileNetworkError());
            return;
        }

        AddSuccess(new DownloadFileSuccess(request.Url, request.Path));
    }
}