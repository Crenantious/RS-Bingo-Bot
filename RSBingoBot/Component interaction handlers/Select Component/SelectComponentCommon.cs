using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RSBingo_Common.General;

namespace RSBingoBot.Component_interaction_handlers.Select_Component
{
    internal class SelectComponentCommon
    {
        // If there's more than this many options, the number of pages they're split into will exceed MaxOptionsPerSelectMenu.
        private static float maxOptions = MathF.Pow(MaxOptionsPerSelectMenu, 2);

        /// <summary>
        /// Tries to convert <paramref name="options"/> into pages, filling the pages with <paramref name="options"/>
        /// starting from the first.
        /// </summary>
        /// <param name="options">The options to try and convert to pages.</param>
        /// <returns><paramref name="options"/> split into a list of pages if <paramref name="options"/>.Count() >
        /// <see cref="MaxOptionsPerSelectMenu"/>, or <paramref name="options"/> if not.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static List<SelectComponentOption> TryConvertToPages(IEnumerable<SelectComponentOption> options)
        {
            // TODO: JR - potentially make this method split pages multiple times to all more than maxOptions options,
            // really not necessary for this project though.

            int numberOfOptions = options.Count();
            
            if (numberOfOptions >= maxOptions)
            {
                throw new ArgumentException($"Options count cannot exceed {maxOptions}");
            }

            List<SelectComponentPage> pages = new() { new SelectComponentPage("Page 1")};

            for (int i = 0; i < options.Count(); i++)
            {
                SelectComponentOption option = options.ElementAt(i);

                if (option.GetType() == typeof(SelectComponentPage))
                {
                    SelectComponentPage page = (SelectComponentPage)option;
                    page.Options = TryConvertToPages(page.Options);
                }

                if (pages[^1].Options.Count == MaxOptionsPerSelectMenu)
                {
                    pages.Add(new ("Page " + (pages.Count + 1).ToString()));
                }

                pages[^1].Options.Add(option);
            }

            if (pages.Count == 1) { return pages[^1].Options; }
            return pages.Cast<SelectComponentOption>().ToList();
        }
    }
}
