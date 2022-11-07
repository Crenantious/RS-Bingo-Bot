// <copyright file="ChangeTileButtonHanlder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Component_interaction_handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using RSBingo_Framework.Models;
    using RSBingoBot.Discord_event_handlers;
    using static RSBingo_Common.General;
    using static RSBingo_Framework.Records.BingoTaskRecord;

    // TODO: JR - make it so when someone is interacting with the button, no-one else can. This prevents conflicts.

    /// <summary>
    /// Handles the interaction with the "Change tile" button in a team's board channel.
    /// </summary>
    public class ChangeTileButtonHanlder : ComponentInteractionHandler
    {
        private const string PageOptionPrefix = "Page ";

        private string fromTileSelectId = string.Empty;
        private string toTileSelectId = string.Empty;
        private string submitButtonId = string.Empty;
        private string? fromTileSelectedOption = null;
        private SelectStage selectStage = SelectStage.Difficulty;
        private IEnumerable<BingoTask>? tasks = null;
        private Difficulty? difficultySelected = null;
        private int pageSelected = -1;
        private string taskSelected = string.Empty;
        private List<DiscordSelectComponentOption> fromTileSelectOptions = new ();
        private DiscordMessage originalResponse = null!;
        private DiscordButtonComponent submitButton = null!;

        /// <inheritdoc/>
        protected override bool ContinueWithNullUser { get { return false; } }

        /// <inheritdoc/>
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            await base.InitialiseAsync(args, info);

            fromTileSelectId = Guid.NewGuid().ToString();
            toTileSelectId = Guid.NewGuid().ToString();
            submitButtonId = Guid.NewGuid().ToString();

            submitButton = new DiscordButtonComponent(ButtonStyle.Primary, submitButtonId, "Submit");

            var builder = new DiscordInteractionResponseBuilder()
                .WithContent($"{args.User.Mention} Loading tiles...");

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
            originalResponse = args.Interaction.GetOriginalResponseAsync().Result;
            MessagesForCleanup.Add(originalResponse);

            await UpdateMessage(args);
            await UpdateMessage(args);

            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: fromTileSelectId), FromTileSelectInteracted);
            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: toTileSelectId), ToTileSelectInteracted);
            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: submitButtonId), SubmitButtonInteracted);
        }

        private DiscordSelectComponent GetFromTileSelectMenu(ComponentInteractionCreateEventArgs args)
        {
            if (fromTileSelectOptions.Count == 0)
            {
                // Team will not be null if TeamMustExist == true, which it should be for this instance.
                string taskName = string.Empty;
                fromTileSelectOptions = new();
                for (int i = 0; i < TilesPerRow * TilesPerColumn; i++)
                {
                    taskName = i < Team.Tiles.Count ?
                        Team.Tiles.ElementAt(i).Task.Name :
                        "No task " + i.ToString();

                    fromTileSelectOptions.Add(new DiscordSelectComponentOption(taskName, taskName));
                }
            }

            string placeholder = fromTileSelectedOption ?? "Change from";
            return new DiscordSelectComponent(fromTileSelectId, placeholder, fromTileSelectOptions, false, 1, 1);
        }

        private DiscordSelectComponent GetToTileSelectMenu(ComponentInteractionCreateEventArgs args)
        {
            var stringOptions = GetToTileSelectOptions();
            var options = new List<DiscordSelectComponentOption>();
            stringOptions = stringOptions.Except(Team.Tiles.Select(t => t.Task.Name));

            foreach (string option in stringOptions)
            {
                options.Add(new(option, option));
            }

            string placeholder = string.Empty;
            if (difficultySelected != null) { placeholder += difficultySelected.ToString(); }
            if (pageSelected >= 0) { placeholder += ", page " + pageSelected.ToString(); }

            return new DiscordSelectComponent(toTileSelectId, placeholder, options, false, 1, 1);
        }

        private IEnumerable<string> GetToTileSelectOptions()
        {
            if (difficultySelected == null)
            {
                selectStage = SelectStage.Difficulty;
                return Enum.GetNames(typeof(Difficulty)).ToList();
            }

            if (tasks == null)
            {
                tasks = DataWorker.BingoTasks.GetAllWithDifficulty((Difficulty)difficultySelected);
            }

            if (pageSelected >= 0)
            {
                selectStage = SelectStage.Task;
                return tasks.Skip(MaxOptionsPerSelectMenu * pageSelected).Take(MaxOptionsPerSelectMenu)
                    .Select(t => t.Name);
            }

            if (tasks.Count() > MaxOptionsPerSelectMenu)
            {
                selectStage = SelectStage.Page;

                List<string> options = new();
                int pages = (int)MathF.Ceiling(tasks.Count() / (float)MaxOptionsPerSelectMenu);

                for (int i = 0; i < pages; i++)
                {
                    options.Add(PageOptionPrefix + i.ToString());
                }
                return options;
            }
            else
            {
                selectStage = SelectStage.Task;
                return tasks.Select(t => t.Name).ToList();
            }
        }

        private async Task UpdateMessage(ComponentInteractionCreateEventArgs args)
        {
            var builder = new DiscordMessageBuilder()
                .WithContent(args.User.Mention)
                .AddComponents(GetFromTileSelectMenu(args))
                .AddComponents(GetToTileSelectMenu(args))
                .AddComponents(submitButton);

            await originalResponse.ModifyAsync(builder);
        }

        private async Task FromTileSelectInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            fromTileSelectedOption = args.Values[0];
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }

        private async Task ToTileSelectInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

            switch (selectStage)
            {
                case SelectStage.Difficulty:
                    foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                    {
                        if (difficulty.ToString() == args.Values[0])
                        {
                            difficultySelected = difficulty;
                            break;
                        }
                    }
                    await UpdateMessage(args);
                    break;

                case SelectStage.Page:
                    pageSelected = int.Parse(args.Values[0][PageOptionPrefix.Length..]);
                    await UpdateMessage(args);
                    break;

                case SelectStage.Task:
                    taskSelected = args.Values[0];
                    break;
            }
        }

        private async Task SubmitButtonInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            string content;

            if (fromTileSelectedOption == null || taskSelected == string.Empty)
            {
                 content = "Must select both a tile to change from, and to change to.";
            }
            else
            {
                content = $"{fromTileSelectedOption} has been changed to {taskSelected}.";
                await InteractionConcluded();
            }

            var builder = new DiscordInteractionResponseBuilder() { }
                .WithContent(content)
                .AsEphemeral();

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
        }

        enum SelectStage
        {
            Difficulty,
            Page,
            Task
        }
    }
}