using System;
using System.Collections.Generic;
using System.Collections.Immutable;

public static class WhitelistChecker
{
    private const string NotInitialised = "WhitelistChecker not initialised. Call Initialise() method first.";
    private static ImmutableHashSet<string>? _whitelistedDomains;

    public static void Initialize(IEnumerable<string> whitelistedDomains)
    {
        _whitelistedDomains = whitelistedDomains.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public static bool IsUrlWhitelisted(string url)
    {
        if (_whitelistedDomains == null)
        {
            throw new InvalidOperationException(NotInitialised);
        }

        if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            return _whitelistedDomains.Contains(uri.Host);
        }

        return false;
    }
}