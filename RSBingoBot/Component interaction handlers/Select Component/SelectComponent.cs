using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSBingoBot.Discord_event_handlers;
using DSharpPlus;

namespace RSBingoBot.Component_interaction_handlers.Select_Component
{
    // TODO: JR - support multiple item selection
    public class SelectComponent
    {
        /// <summary>
        /// Gets the discord version of the component. This is null if either <see cref="Build"/> has not been called, or it failed.
        /// </summary>
        public DiscordSelectComponent? DiscordComponent { get; private set; }
        public SelectComponentPage? SelectedPage { get; private set; } = null;
        public List<SelectComponentItem> SelectedItems { get; private set; } = new();
        public List<SelectComponentOption> Options = new();
        public string CustomId { get; set; } = null!;
        public string InitialPlaceholder { get; set; } = null!;
        public bool Disabled { get; set; }
        public int MinOptions { get; set; }
        public int MaxOptions { get; set; }

        private string placeholder = string.Empty;
        private bool hasBeenInteracted = false;
        private Func<ComponentInteractionCreateEventArgs, Task>? itemSelectedCallback;
        private Func<ComponentInteractionCreateEventArgs, Task>? pageSelectedCallback;
        private List<DiscordSelectComponentOption> discordOptions = new();
        private HashSet<SelectComponentItem> SelectedItemsHashSet = new();

        public SelectComponent(string customId, string placeholder,
            Func<ComponentInteractionCreateEventArgs, Task>? itemSelectedCallback = null,
            Func<ComponentInteractionCreateEventArgs, Task>? pageSelectedCallback = null,
            bool disabled = false, int minOptions = 1, int maxOptions = 1)
        {
            this.CustomId = customId;
            this.itemSelectedCallback = itemSelectedCallback;
            this.pageSelectedCallback = pageSelectedCallback;
            this.Disabled = disabled;
            this.MinOptions = minOptions;
            this.MaxOptions = maxOptions;
            UpdatePlaceholder(placeholder);
        }

        /// <summary>
        /// Builds the structure of the pages and items, creates the corresponding <see cref="DiscordSelectComponentOption"/>s
        /// and the <see cref="DiscordSelectComponent"/>.
        /// This should be called before <see cref="OnInteraction"/>.
        /// </summary>
        public void Build()
        {
            // TODO: JR - add support for building multiple times.
            Options = SelectComponentCommon.TryConvertToPages(Options);
            BuildAllDiscordOptions();
            CreateDiscordSelectComponent();
        }

        /// <summary>
        /// This should be subscribed to the <see cref="ComponentInteractionDEH"/> so it is called when
        /// the <see cref="DiscordSelectComponent"/> is interacted with.
        /// </summary>
        /// <param name="client">The client the interaction occurred on.</param>
        /// <param name="args">The arguments for the interaction</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task OnInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            if (DiscordComponent == null) { throw new NullReferenceException($"Component must not be null; make sure Build() has been called before this method."); }

            hasBeenInteracted = true;
            (List<SelectComponentOption> options, bool pageSelected)  = GetOptionsFromInteractonArgs(args);

            if (pageSelected)
            {
                await PageSlected(args, (SelectComponentPage)options[0]);
                return;
            }

            await ItemSelected(args, options.Cast<SelectComponentItem>());
        }

        private (List<SelectComponentOption>, bool) GetOptionsFromInteractonArgs(ComponentInteractionCreateEventArgs args)
        {
            List<SelectComponentOption> options = new(args.Values.Length);
            SelectComponentOption option;
            int index;

            for (int i = 0; i < args.Values.Length; i++)
            {
                try
                {
                    index = int.Parse(args.Values[i]);
                    option = Options[index];
                }
                catch
                {
                    // Received garbage data
                    continue;
                }

                if (option.GetType().IsAssignableFrom(typeof(SelectComponentPage)))
                {
                    options = new() { option };

                    // If a page is selected with an item or another page, the first page will be loaded and the other
                    // selections will be ignored.
                    return (options, true);
                }

                options.Add(option);
            }

            return (options, false);
        }

        private void BuildAllDiscordOptions()
        {
            discordOptions.Clear();

            for (int i = 0; i < Options.Count; i++)
            {
                SelectComponentOption option = Options[i];
                option.isDefault = SelectedItemsHashSet.Contains(option);
                option.Build(i.ToString());
                discordOptions.Add(option.discordOption);
            }
        }

        private void BuildDefaultDiscordOptions()
        {
            for (int i = 0; i < Options.Count; i++)
            {
                SelectComponentOption option = Options[i];

                if (SelectedItemsHashSet.Contains(option))
                {
                    option.isDefault = true;
                    option.Build(i.ToString());
                    discordOptions[i] = option.discordOption;
                }
            }
        }

        private void CreateDiscordSelectComponent()
        {
            string p = placeholder == string.Empty ? InitialPlaceholder : placeholder;
            int maxOptions = (int)MathF.Min(MaxOptions, discordOptions.Count());

            // For maxOptions, the number cannot exceed the amount of discordOptions or there'll be an error
            DiscordComponent = new DiscordSelectComponent(CustomId, p, discordOptions, Disabled, MinOptions, maxOptions);
        }

        private async Task PageSlected(ComponentInteractionCreateEventArgs args, SelectComponentPage page)
        {
            SelectedPage = page;
            Options = page.Options;
            UpdatePlaceholder(page.label);
            SelectedItems.Clear();
            SelectedItemsHashSet.Clear();
            Build();

            if (pageSelectedCallback != null)
            {
                await pageSelectedCallback.Invoke(args);
            }
        }

        private async Task ItemSelected(ComponentInteractionCreateEventArgs args, IEnumerable<SelectComponentItem> items)
        {
            SelectedPage = null;
            SelectedItems = new(items);
            SelectedItemsHashSet = new(items);
            BuildDefaultDiscordOptions();
            CreateDiscordSelectComponent();

            if (itemSelectedCallback != null)
            {
                await itemSelectedCallback(args);
            }
        }

        private void UpdatePlaceholder(string pageLabel)
        {
            if (!hasBeenInteracted)
            {
                InitialPlaceholder = pageLabel;
                placeholder = pageLabel;
            }
            else if (placeholder == InitialPlaceholder)
            {
                placeholder = pageLabel;
            }
            else
            {
                placeholder += ", " + pageLabel;
            }
        }
    }
}
