// <copyright file="WhitelistChecker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Web;

using System.Collections.Immutable;

public static class WhitelistChecker
{
    private const string NotInitialised = "WhitelistChecker is not initialised. Call Initialise() method first.";

    private static ImmutableHashSet<string>? domainsHashSet;
    private static IEnumerable<string>? domainsEnumerable;

    public static void Initialise(IEnumerable<string> whitelistedDomains)
    {
        domainsEnumerable = whitelistedDomains;
        domainsHashSet = whitelistedDomains.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if the given url is from a whitelisted domain.
    /// </summary>
    /// <returns><see langword="true"/> if the url is permitted, <see langword="false"/> otherwise.</returns>
    /// <exception cref="InvalidOperationException">If the permitted domains list is not initialised</exception>
    public static bool IsUrlWhitelisted(string url)
    {
        CheckInitialised();

        if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            return domainsHashSet!.Contains(uri.Host);
        }

        return false;
    }

    /// <summary>
    /// Checks if the given uri is from a whitelisted domain.
    /// </summary>
    /// <returns><see langword="true"/> if the uri is permitted, <see langword="false"/> otherwise.</returns>
    /// <exception cref="InvalidOperationException">If the permitted domains list is not initialised</exception>
    public static bool IsUriWhitelisted(Uri uri)
    {
        CheckInitialised();
        return domainsHashSet!.Contains(uri.Host);
    }

    /// <returns>All the permitted domains</returns>
    /// <exception cref="InvalidOperationException">If the permitted domains list is not initialised</exception>
    public static IEnumerable<string> GetWhitelistedDomains()
    {
        if (domainsEnumerable is null)
        {
            throw new InvalidOperationException(NotInitialised);
        }

        return domainsEnumerable.ToArray();
    }

    private static void CheckInitialised()
    {
        if (domainsHashSet is null)
        {
            throw new InvalidOperationException(NotInitialised);
        }
    }
}