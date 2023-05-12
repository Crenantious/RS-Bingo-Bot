// <copyright file="WhitelistChecker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework;

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

    public static bool IsUrlWhitelisted(string url)
    {
        if (domainsHashSet is null)
        {
            throw new InvalidOperationException(NotInitialised);
        }

        if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            return domainsHashSet.Contains(uri.Host);
        }

        return false;
    }

    public static IEnumerable<string> GetWhitelistedDomains()
    {
        if (domainsEnumerable is null)
        {
            throw new InvalidOperationException(NotInitialised);
        }

        return domainsEnumerable.ToArray();
    }
}