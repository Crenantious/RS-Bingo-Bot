// <copyright file="WebServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using FluentResults;
using RSBingoBot.Requests;

public class WebServices : RequestService, IWebServices
{
    /// <param name="path">The local path to save the file to.</param>
    public async Task<Result> DownloadFile(string url, string path) =>
        await RunRequest(new DownloadFileRequest(url, path));
}