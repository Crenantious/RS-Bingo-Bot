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
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using RSBingoBot.Discord_event_handlers;
    using RSBingoBot.Imaging;
    using SixLabors.ImageSharp;
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
        private const string NoTaskName = "No task";

        private string fromTileSelectId = string.Empty;
        private string? fromTileSelectedTileId = null;
        private string fromTileSelectPlaceholder = "Change from";
        private List<DiscordSelectComponentOption> fromTileSelectOptions = new();

        private string toTileSelectId = string.Empty;
        private int toTaskSelectedTileId = -1;

        private int? pageSelected = null;
        private Difficulty difficultySelected;
        private SelectStage selectStage = SelectStage.None;
        private ToSelectOptions toSelectOptions;

        private string submitButtonId = string.Empty;
        private DiscordButtonComponent submitButton = null!;
        private DiscordMessage originalResponse = null!;

        /// <inheritdoc/>
        protected override bool ContinueWithNullUser { get { return false; } }

        /// <inheritdoc/>
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            await base.InitialiseAsync(args, info);

            fromTileSelectId = Guid.NewGuid().ToString();
            toTileSelectId = Guid.NewGuid().ToString();
            submitButtonId = Guid.NewGuid().ToString();

            var builder = new DiscordInteractionResponseBuilder()
                .WithContent($"{args.User.Mention} Loading tiles...");

            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);

            toSelectOptions = new(DataWorker, Team!);
            SetSelectStage();

            submitButton = new DiscordButtonComponent(ButtonStyle.Primary, submitButtonId, "Submit");

            originalResponse = args.Interaction.GetOriginalResponseAsync().Result;
            MessagesForCleanup.Add(originalResponse);
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
                fromTileSelectOptions = new();
                IEnumerable<Tile> tiles = Team!.Tiles.OrderBy(t => t.BoardIndex);
                int tileIndex = 0;
                string name;
                string value;

                for (int i = 0; i < TilesPerRow * TilesPerColumn; i++)
                {
                    name = NoTaskName;
                    value = NoTaskName + i.ToString();

                    if (tileIndex < tiles.Count() && tiles.ElementAt(tileIndex).BoardIndex == i)
                    {
                        name = tiles.ElementAt(tileIndex).Task.Name;
                        value = tiles.ElementAt(tileIndex).RowId.ToString();
                        tileIndex++;
                    }

                    fromTileSelectOptions.Add(new (name, value));
                }
            }
            else
            {
                for (int i = 0; i < fromTileSelectOptions.Count; i++)
                {
                    if (fromTileSelectedTileId == fromTileSelectOptions[i].Value)
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
            List<DiscordSelectComponentOption> options = GetToTileSelectOptions();
            string placeholder = string.Empty;

            if (selectStage != SelectStage.Difficulty) { placeholder += difficultySelected.ToString(); }
            if (pageSelected != null) { placeholder += ", page " + pageSelected.ToString(); }

            return new DiscordSelectComponent(toTileSelectId, placeholder, options, false, 1, 1);
        }

        private List<DiscordSelectComponentOption> GetToTileSelectOptions() =>
            selectStage switch
            {
                SelectStage.NoTasksAvailable => new() { new("No tasks available.", "No tasks available.") },
                SelectStage.Difficulty => toSelectOptions.GetdifficultyOptions(),
                SelectStage.Page => toSelectOptions.GetPageOptions(difficultySelected),
                SelectStage.Task => toSelectOptions.GetTaskOptions(difficultySelected, pageSelected ?? 0),
                _ => throw new ArgumentOutOfRangeException(nameof(selectStage), $"Not expected {nameof(SelectStage)} value: {selectStage}")
            };

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
            fromTileSelectedTileId = args.Values[0];
            fromTileSelectPlaceholder = fromTileSelectedTileId.StartsWith(NoTaskName) ?
                NoTaskName :
                DataWorker.Tiles.GetById(int.Parse(fromTileSelectedTileId)).Task.Name;
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

                SetSelectStage();
                await UpdateMessage(args);
            }
            else if (selectStage == SelectStage.Page)
            {
                // TODO: add verification rules for all data retrieved from Discord
                pageSelected = int.Parse(args.Values[0][PageOptionPrefix.Length..]);
                SetSelectStage();
                await UpdateMessage(args);
            }
            else if (selectStage == SelectStage.Task)
            {
                toTaskSelectedTileId = int.Parse(args.Values[0]);
            }
        }

        private void SetSelectStage()
        {
            if (toSelectOptions.GetdifficultyOptions().Count == 0)
            {
                selectStage = SelectStage.NoTasksAvailable;
            }
            else if (selectStage == SelectStage.None)
            {
                selectStage = SelectStage.Difficulty;
            }
            else if (selectStage == SelectStage.Difficulty)
            {
                int pageCount = toSelectOptions.GetPageOptions(difficultySelected).Count;
                selectStage = pageCount > 1 ?
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
            var builder = new DiscordInteractionResponseBuilder() { }
                .WithContent("Submitting...")
                .AsEphemeral();
            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);

            string editBuilderContent;

            if (selectStage == SelectStage.NoTasksAvailable)
            {
                editBuilderContent = "No tasks available.";
            }
            else if (fromTileSelectedTileId == null || toTaskSelectedTileId == -1)
            {
                 editBuilderContent = "Must select a tile to change both from, and to.";
            }
            else
            {
                // TODO: hook up tile changes with the update handler
                BingoTask toTask = DataWorker.BingoTasks.GetById(toTaskSelectedTileId);

                if (fromTileSelectedTileId.StartsWith(NoTaskName))
                {
                    DataWorker.Tiles.Create(Team, toTask, int.Parse(fromTileSelectedTileId[NoTaskName.Length..]));
                    editBuilderContent = $"{NoTaskName} has been changed to {toTask.Name}.";
                }
                else
                {
                    Tile fromTile = DataWorker.Tiles.GetById(int.Parse(fromTileSelectedTileId));
                    Tile? toTile = DataWorker.Tiles.GetByTeamAndTaskId(Team, toTask.RowId);

                    if (toTile != null)
                    {
                        if (toTile == fromTile)
                        {
                            editBuilderContent = $"You selected the same tile twice, so nothing happened.";
                        }
                        else
                        {
                            //DataWorker.Tiles.SwapTasks(fromTile, toTile);
                            UpdateBoard(fromTile, toTile);
                            editBuilderContent = $"{fromTile.Task.Name} and {toTask.Name} have been successfully swapped.";
                        }
                    }
                    else
                    {
                        //DataWorker.Tiles.ChangeTask(fromTile, toTask);
                        UpdateBoard(fromTile);
                        editBuilderContent = $"{fromTile.Task.Name} has been changed to {toTask.Name}.";
                    }
                }

                await InteractionConcluded();
            }

            var editBuilder = new DiscordWebhookBuilder() { }
                .WithContent(editBuilderContent);
            await args.Interaction.EditOriginalResponseAsync(editBuilder);

            await InteractionConcluded();
        }

        private async Task UpdateBoard(params Tile[] tilesChanged)
        {
            foreach (Tile tile in tilesChanged)
            {
                BoardImage.UpdateTileTask(tile);
            }

            await InitialiseTeam.UpdateBoard(Team, BoardImage.GetTeamBoard(Team.RowId));
        }

        enum SelectStage
        {
            None,
            Difficulty,
            Page,
            Task,
            NoTasksAvailable
        }

        // TODO: JR - make an instance of this be statically stored and auto updated whenever tasks
        // are added or deleted. More efficient then creating a new object with each button interaction.
        private struct ToSelectOptions
        {
            private readonly DiscordComponentEmoji onBoardEmoji = new ("👍");
            private readonly IDataWorker dataWorker;
            private readonly Team team;
            private readonly HashSet<int> taskIdsOnBoard = new();
            private readonly Dictionary<sbyte, List<List<DiscordSelectComponentOption>>> taskOptions = new();
            private readonly Dictionary<sbyte, List<DiscordSelectComponentOption>> pageOptions = new();
            private readonly List<DiscordSelectComponentOption> difficultyOptions = new();

            public ToSelectOptions(IDataWorker dataWorker, Team team)
            {
                this.dataWorker = dataWorker;
                this.team = team;
                HashTasksOnBoard();
                AddTasksAndPages();
            }

            public List<DiscordSelectComponentOption> GetdifficultyOptions() =>
                difficultyOptions;

            public List<DiscordSelectComponentOption> GetPageOptions(Difficulty difficulty) =>
                pageOptions[(sbyte)difficulty];

            public List<DiscordSelectComponentOption> GetTaskOptions(Difficulty difficulty, int pageNumber) =>
                taskOptions[(sbyte)difficulty][pageNumber];

            public bool DoesDifficultyExist(Difficulty difficulty) =>
                taskOptions.ContainsKey((sbyte)difficulty);

            public bool IsTaskOnBoard(int taskId) =>
                taskIdsOnBoard.Contains(taskId);

            private void AddTasksAndPages()
            {
                IEnumerable<BingoTask> tasks = dataWorker.BingoTasks.GetAllTasks();
                foreach (BingoTask task in tasks)
                {
                    if (!taskOptions.ContainsKey(task.Difficulty)) { AddDifficulty(task.Difficulty); }

                    List<DiscordSelectComponentOption> page = taskOptions[task.Difficulty][^1];
                    if (page.Count == MaxOptionsPerSelectMenu)
                    {
                        AddPage(task.Difficulty);
                        page = taskOptions[task.Difficulty][^1];
                    }

                    DiscordComponentEmoji? emoji = taskIdsOnBoard.Contains(task.RowId) ?
                        onBoardEmoji :
                        null;
                    page.Add(new(task.Name, task.RowId.ToString(), emoji: emoji));
                }
            }

            private void AddPage(sbyte difficulty)
            {
                taskOptions[difficulty].Add(new());
                string pageName = "Page " + pageOptions[difficulty].Count().ToString();
                pageOptions[difficulty].Add(new(pageName, pageName));
            }

            private void HashTasksOnBoard()
            {
                foreach (Tile tile in team.Tiles)
                {
                    taskIdsOnBoard.Add(tile.TaskId);
                }
            }

            private void AddDifficulty(sbyte difficulty)
            {
                taskOptions.Add(difficulty, new());
                pageOptions.Add(difficulty, new());
                AddPage(difficulty);

                string difficultyName = ((Difficulty)difficulty).ToString();
                difficultyOptions.Add(new(difficultyName, difficultyName));
            }
        }
    }
}