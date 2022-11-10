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
    using static RSBingo_Framework.Records.TileRecord;

    // TODO: JR - make it so when someone is interacting with the button, no-one else can. This prevents conflicts.

    /// <summary>
    /// Handles the interaction with the "Change tile" button in a team's board channel.
    /// </summary>
    public class ChangeTileButtonHanlder : ComponentInteractionHandler
    {
        private const string PageOptionPrefix = "Page ";
        private const string NoTaskPrefix = "No task ";

        private string fromTileSelectId = string.Empty;
        private string toTileSelectId = string.Empty;
        private string submitButtonId = string.Empty;
        private string? fromTileSelectedTile = null;
        private string fromTileSelectPlaceholder = "Change from";
        private int toTileSelectedTileRowId = -1;
        private SelectStage selectStage = SelectStage.Difficulty;
        private List<IEnumerable<BingoTask>>? tasksWithSelectedDifficulty = null;
        private Difficulty difficultySelected;
        private int pageSelected = -1;
        private List<DiscordSelectComponentOption> fromTileSelectOptions = new();
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
                string name = string.Empty;
                string value = string.Empty;
                fromTileSelectOptions = new();

                for (int i = 0; i < TilesPerRow * TilesPerColumn; i++)
                {
                    if (i < Team.Tiles.Count)
                    {
                        name = Team.Tiles.ElementAt(i).Task.Name;
                        value = Team.Tiles.ElementAt(i).RowId.ToString();
                    }
                    else
                    {
                        name = NoTaskPrefix;
                        value = NoTaskPrefix + i.ToString();
                    }

                    fromTileSelectOptions.Add(new DiscordSelectComponentOption(name, value, isDefault: value == fromTileSelectedTile));
                }
            }
            else
            {
                for (int i = 0; i < fromTileSelectOptions.Count; i++)
                {
                    if (fromTileSelectedTile == fromTileSelectOptions[i].Value)
                    {
                        fromTileSelectOptions[i] = new(
                            fromTileSelectOptions[i].Label,
                            fromTileSelectOptions[i].Value,
                            isDefault: true);
                    }
                }
            }

            return new DiscordSelectComponent(fromTileSelectId, fromTileSelectPlaceholder, fromTileSelectOptions, false, 1, 1);
        }

        private DiscordSelectComponent GetToTileSelectMenu(ComponentInteractionCreateEventArgs args)
        {
            (IEnumerable<string> optionNames, IEnumerable<int>? optionValues) = GetToTileSelectOptions();
            var options = new List<DiscordSelectComponentOption>();

            for (int i = 0; i < optionNames.Count(); i++)
            {
                string value = optionValues == null ?
                    optionNames.ElementAt(i) :
                    optionValues.ElementAt(i).ToString();

                options.Add(new(optionNames.ElementAt(i), value));
            }

            string placeholder = string.Empty;
            if (selectStage != SelectStage.Difficulty) { placeholder += difficultySelected.ToString(); }
            if (pageSelected >= 0) { placeholder += ", page " + pageSelected.ToString(); }

            return new DiscordSelectComponent(toTileSelectId, placeholder, options, false, 1, 1);
        }

        /// <summary>
        /// Gets the option names and values for the toTile select component.
        /// </summary>
        /// <returns>(option names, option values). If option values is null then use the option name as the value.</returns>
        private (IEnumerable<string>, IEnumerable<int>?) GetToTileSelectOptions()
        {
            if (selectStage == SelectStage.Difficulty)
            {
                return (Enum.GetNames(typeof(Difficulty)), null);
            }
            else if (selectStage == SelectStage.Page)
            {
                // tasksWithSelectedDifficulty will be set when the difficulty is selected, so this will not be null here.
                List<string> names = new();

                for (int i = 0; i < tasksWithSelectedDifficulty.Count; i++)
                {
                    names.Add(PageOptionPrefix + i.ToString());
                }

                return (names, null);
            }
            else if (selectStage == SelectStage.Task)
            {
                // tasksWithSelectedDifficulty will be set when the difficulty is selected, so this will not be null here.
                int index = pageSelected >= 0 ? pageSelected : 0;
                List<string> names = new();
                List<int> rowIds = new();

                foreach (BingoTask bingoTask in tasksWithSelectedDifficulty[index])
                {
                    names.Add(bingoTask.Name);
                    rowIds.Add(bingoTask.RowId);
                }

                return (names, rowIds);
            }
            return (new List<string>(), null);
        }

        private void SetTasks()
        {
            if (tasksWithSelectedDifficulty != null) { return; }

            IEnumerable<BingoTask> tasks = DataWorker.BingoTasks.GetAllWithDifficulty(difficultySelected).ToList();

            if (tasks.Count() == 0) { throw new NotImplementedException("No tasks with the given difficulty could be found"); }

            tasksWithSelectedDifficulty = new List<IEnumerable<BingoTask>>();
            int pages = (int)MathF.Ceiling(tasks.Count() / (float)MaxOptionsPerSelectMenu);

            for (int i = 0; i < pages; i++)
            {
                tasksWithSelectedDifficulty.Add(tasks
                    .Skip(MaxOptionsPerSelectMenu * i)
                    .Take(MaxOptionsPerSelectMenu));
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
            fromTileSelectedTile = args.Values[0];

            fromTileSelectPlaceholder = fromTileSelectedTile.StartsWith(NoTaskPrefix) ?
                NoTaskPrefix :
                DataWorker.Tiles.GetById(int.Parse(fromTileSelectedTile)).Task.Name;

            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }

        private async Task ToTileSelectInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            await args.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

            if (selectStage == SelectStage.Difficulty)
            {
                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    if (difficulty.ToString() == args.Values[0])
                    {
                        difficultySelected = difficulty;
                        break;
                    }
                }

                SetTasks();
                SetSelectStage();
                await UpdateMessage(args);
            }
            else if (selectStage == SelectStage.Page)
            {
                pageSelected = int.Parse(args.Values[0][PageOptionPrefix.Length..]);
                SetSelectStage();
                await UpdateMessage(args);
            }
            else if (selectStage == SelectStage.Task)
            {
                toTileSelectedTileRowId = int.Parse(args.Values[0]);
            }
        }

        private void SetSelectStage()
        {
            if (selectStage == SelectStage.Difficulty)
            {
                selectStage = tasksWithSelectedDifficulty.Count > 1 ?
                   SelectStage.Page :
                   SelectStage.Task;
            }
            else if (selectStage == SelectStage.Page)
            {
                selectStage = SelectStage.Task;
            }
        }

        private async Task SubmitButtonInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            string content;

            if (fromTileSelectedTile == null || toTileSelectedTileRowId == -1)
            {
                 content = "Must select a tile to change both from, and to.";
            }
            else
            {
                BingoTask? toTask = DataWorker.BingoTasks.GetById(toTileSelectedTileRowId);
                if (fromTileSelectedTile.StartsWith(NoTaskPrefix))
                {
                    DataWorker.Tiles.Create(Team.Name, toTask, VerifiedStatus.No);
                }
                else
                {
                    DataWorker.Tiles.GetById(int.Parse(fromTileSelectedTile)).Task = toTask;
                }

                DataWorker.SaveChanges();
                content = $"{fromTileSelectPlaceholder} has been changed to {toTask.Name}.";
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