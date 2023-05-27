// <copyright file="WebRequests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using System.Net;
using RSBingo_Framework;
using RSBingo_Framework.DTO;
using RSBingo_Framework.Exceptions;

public static class WebRequests
{
    private const string UnableToDownloadFileResponse = "Unable to download the file. Please try again shortly.";

    /// <summary>
    /// Downloads a file from the <paramref name="url"/> and saves it to disk at <paramref name="path"/>.
    /// </summary>
    /// <returns><see cref="Result"/> can contain the following exceptions:<br/>
    /// <see cref="UnpermittedURLException"/> if the <paramref name="url"/> is not from a whitelisted domain.<br/>
    /// <see cref="InvalidOperationException"/> if <see cref="WhitelistChecker"/> is not initialised.<br/>
    /// <see cref="WebException"/> if the <paramref name="url"/> cannot be accessed.<br/>
    /// <see cref="ArgumentException"/> if the <paramref name="url"/> or <paramref name="path"/> are null/empty.<br/>
    /// <see cref="NotSupportedException"/> if this is being ran on multiple threads.</returns>
    public static Result DownloadFile(string url, string path)
    {
        WebClient webClient = new();

        try
        {
            if (WhitelistChecker.IsUrlWhitelisted(url) is false)
            {
                throw new UnpermittedURLException(WhitelistChecker.GetWhitelistedDomains());
            }
            webClient.DownloadFile(url, path);
        }
        catch (WebException e)
        {
            return new(new WebException(UnableToDownloadFileResponse));
        }
        catch (Exception e)
        {
            return new(e);
        }

        return new();
    }
}