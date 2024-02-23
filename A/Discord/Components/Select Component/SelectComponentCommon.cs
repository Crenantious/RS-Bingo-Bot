// <copyright file="SelectComponentCommon.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using static RSBingo_Common.General;

internal class SelectComponentCommon
{
    // If there's more than this many options, the number of pages they're split into will exceed MaxOptionsPerSelectMenu.
    private static int maxOptions = (int)MathF.Pow(MaxSelectOptionsPerPage, 2);

    /// <summary>
    /// Tries to split <paramref name="options"/> into pages, filling the pages with <paramref name="options"/>
    /// starting from the first. The amount cannot exceed <see cref="MaxSelectOptionsPerPage"/>^2.
    /// </summary>
    /// <returns><paramref name="options"/> split into a list of pages if <paramref name="options"/>.Count() >
    /// <see cref="MaxSelectOptionsPerPage"/>, or <paramref name="options"/> if not.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<SelectComponentOption> TryConvertToPages(IEnumerable<SelectComponentOption> options,
        SelectComponentGetPageName getPageLabel)
    {
        int optionsCount = options.Count();

        if (optionsCount <= MaxSelectOptionsPerPage)
        {
            return options;
        }

        if (optionsCount >= maxOptions)
        {
            throw new ArgumentException($"Options count cannot exceed {maxOptions}");
        }

        return ConvertToPages(options, getPageLabel, optionsCount);
    }

    private static IEnumerable<SelectComponentOption> ConvertToPages(IEnumerable<SelectComponentOption> options,
        SelectComponentGetPageName getPageName, int optionsCount)
    {
        List<SelectComponentPage> pages = new();

        for (int i = 0; i < (float)optionsCount / MaxSelectOptionsPerPage; i++)
        {
            var pageOptions = options.Skip(i * MaxSelectOptionsPerPage)
                .Take(MaxSelectOptionsPerPage);
            pages.Add(new(getPageName, pageOptions));
        }

        return pages;
    }
}