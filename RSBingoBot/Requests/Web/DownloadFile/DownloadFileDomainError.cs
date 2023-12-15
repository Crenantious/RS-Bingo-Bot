// <copyright file="DownloadFileDomainError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingoBot.Web;
using System.Text;

internal class DownloadFileDomainError : Error
{
    private const string ErrorMessage = "The url must be from a whitelisted domain:";
    private const string NoWhitelistedDomainsError = "Unable to download file; no whitelisted domains found.";

    public DownloadFileDomainError() : base(GetMessage())
    {

    }

    private static string GetMessage()
    {
        IEnumerable<string> domains = WhitelistChecker.GetWhitelistedDomains();
        if (domains.Any() is false)
        {
            return NoWhitelistedDomainsError;
        }

        StringBuilder message = new(ErrorMessage);
        domains.ForEach(d => message.Append(Environment.NewLine + d));
        return message.ToString();
    }
}