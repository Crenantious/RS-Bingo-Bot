// <copyright file="SelectComponentCommon.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using static RSBingo_Common.General;

internal class SelectComponentCommon
{
    private const int PageDepthLimit = 5; // Currently 5 is an arbitrary number. 3 Is the deepest we have seen in practice.

    // If there's more than this many options, the number of pages they're split into will exceed MaxOptionsPerSelectMenu.
    private static int maxOptions = (int)MathF.Pow(MaxOptionsPerSelectMenu, 2);

    /// <summary>
    /// Tries to convert <paramref name="options"/> into pages, filling the pages with <paramref name="options"/>
    /// starting from the first.
    /// </summary>
    /// <param name="options">The options to try and convert to pages.</param>
    /// <returns><paramref name="options"/> split into a list of pages if <paramref name="options"/>.Count() >
    /// <see cref="MaxOptionsPerSelectMenu"/>, or <paramref name="options"/> if not.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static List<SelectComponentOption> FormatSelectComponentOptions(IEnumerable<SelectComponentOption> options, int currentDepth = 0)
    {
        if (currentDepth >= PageDepthLimit)
        {
            throw new ArgumentException($"Page depth is too deep.");
        }

        int numberOfOptions = options.Count();
        
        if (numberOfOptions >= maxOptions)
        {
            throw new ArgumentException($"Options count cannot exceed {maxOptions}");
        }

        List<SelectComponentPage> pages = new() { new SelectComponentPage("Page 1")};

        foreach (SelectComponentOption option in options)
        {
            if (option.GetType() == typeof(SelectComponentPage))
            {
                SelectComponentPage page = (SelectComponentPage)option;
                page.Options = FormatSelectComponentOptions(page.Options, currentDepth + 1);
            }

            if (pages[^1].Options.Count is MaxOptionsPerSelectMenu)
            {
                pages.Add(new ("Page " + (pages.Count + 1).ToString()));
            }

            pages[^1].Options.Add(option);
        }

        return pages.Count switch
        {
            1 => pages[0].Options,
            _ => pages.Cast<SelectComponentOption>().ToList(),
        };
    }
}