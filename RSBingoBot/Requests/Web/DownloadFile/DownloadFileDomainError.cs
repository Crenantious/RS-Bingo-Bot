// <copyright file="DownloadFileDomainError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingoBot.Web;
using System.Text;

internal class DownloadFileDomainError : Error
{
    private const string ErrorMessage = "The url must be from one of the following domains:";

    public DownloadFileDomainError() : base(GetMessage())
    {

    }

    private static string GetMessage()
    {
        StringBuilder message = new(ErrorMessage);
        WhitelistChecker.GetWhitelistedDomains()
            .ForEach(d => message.Append(Environment.NewLine + d));
        return message.ToString();
    }
}