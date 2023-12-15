// <copyright file="DownloadFileNetworkError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class DownloadFileNetworkError : Error
{
    private const string ErrorMessage = "A network error occurred, please try again later.";

    public DownloadFileNetworkError() : base(ErrorMessage)
    {

    }
}