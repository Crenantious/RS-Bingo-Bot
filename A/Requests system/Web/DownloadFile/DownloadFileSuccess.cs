// <copyright file="DownloadFileSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using RSBingo_Common;

internal class DownloadFileSuccess : Success
{
    private const string SuccessMessage = "Downloaded file from url '{0}' to local path '{1}'.";

    public DownloadFileSuccess(string url, string path) : base(SuccessMessage.FormatConst(url, path))
    {

    }
}