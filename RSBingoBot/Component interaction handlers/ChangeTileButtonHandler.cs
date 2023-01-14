// <copyright file="ChangeTileButtonHandler.cs" company="PlaceholderCompany">
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
    using RSBingo_Framework.Records;
    using RSBingoBot.Discord_event_handlers;
    using RSBingoBot.Exceptions;
    using RSBingoBot.Imaging;
    using Select_Component;
    using SixLabors.ImageSharp;
    using static RSBingo_Common.General;
    using static RSBingo_Framework.Records.BingoTaskRecord;
    using static RSBingo_Framework.Records.TileRecord;

    // TODO: JR - make it so when someone is interacting with the button, no-one else can. This prevents conflicts.

    /// <summary>
    /// Handles the interaction with the "Change tile" button in a team's board channel.
    /// </summary>
    public class ChangeTileButtonHandler : ComponentInteractionHandler
    {
        private const string PageOptionPrefix = "Page ";
        private const string NoTaskName = "No task";

        private TileInfo fromTileInfo;
        private TileInfo toTileInfo;

        private string submitButtonId = string.Empty;
        private DiscordButtonComponent submitButton = null!;
        private DiscordMessage originalResponse = null!;

        /// <inheritdoc/>
        protected override bool ContinueWithNullUser { get { return false; } }

        /// <inheritdoc/>
        public async override Task InitialiseAsync(ComponentInteractionCreateEventArgs args, InitialisationInfo info)
        {
            await base.InitialiseAsync(args, info);

            submitButtonId = Guid.NewGuid().ToString();

            originalResponse = args.Interaction.GetOriginalResponseAsync().Result;
            MessagesForCleanup.Add(originalResponse);

            fromTileInfo = new(CreateFromTileSelectComponent());
            toTileInfo = new(CreateToTileSelectComponent());

            submitButton = new DiscordButtonComponent(ButtonStyle.Primary, submitButtonId, "Submit");

            await UpdateMessage(args);

            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: fromTileInfo.SelectComponent.CustomId),
                fromTileInfo.SelectComponent.OnInteraction, false);
            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: toTileInfo.SelectComponent.CustomId),
                toTileInfo.SelectComponent.OnInteraction, false);
            SubscribeComponent(new ComponentInteractionDEH.Constraints(user: args.User, customId: submitButtonId),
                SubmitButtonInteracted, true, "Submitting...");
        }

        /// <summary>
        /// Changes the team's tiles in the database and on the board image.
        /// </summary>
        /// <returns>The message to display to the user notifying them of how the tiles (if any) were changed.</returns>
        public static string ChangeDBTiles(IDataWorker dataWorker, Team team, Tile? tile, BingoTask task, int? boardIndex = null)
        {
            // This method is used in tests, so tile2 must be gotten here.
            Tile? tile2 = GetToTile(team, task);

            if (tile == null)
            {
                if (boardIndex == null)
                {
                    throw new ArgumentNullException(nameof(boardIndex), $"{nameof(boardIndex)} argument cannot be null if {nameof(tile)} is null.");
                }

                if (tile2 == default)
                {
                    TileRecord.CreateTile(dataWorker, team, task, (int)boardIndex);
                    return $"{task.Name} has been added to the board.";
                }

                tile2.BoardIndex = (int)boardIndex;
                return $"{task.Name} has been successfully moved.";
            }

            if (tile2 == null)
            {
                string message = $"{tile.Task.Name} has been changed to {task.Name}.";
                tile.ChangeTask(task);
                return message;
            }

            if (tile.RowId == tile2.RowId)
            {
                return "You selected the same tile twice, so nothing happened.";
            }

            tile.SwapTasks(tile2, dataWorker);
            return $"{tile.Task.Name} has been successfully swapped with {task.Name}.";
        }

        private static Tile? GetToTile(Team team, BingoTask task) =>
            team.Tiles.FirstOrDefault(t => t.Task == task);

        private void UpdateFromTileInfo(ComponentInteractionCreateEventArgs args)
        {
            string? errorMessage = null;

            if (!fromTileInfo.SelectComponent.SelectedItems.Any())
            {
                errorMessage = "Must select a tile to change from.";
            }
            else
            {
                FromTileSelectValue value = (FromTileSelectValue)fromTileInfo.SelectComponent.SelectedItems[0].value!;
                fromTileInfo.BoardIndexAtSelection = value.BoardIndex;

                if (value.TileRowId != null)
                {
                    fromTileInfo.Tile = Team!.Tiles.FirstOrDefault(t => t.RowId == value.TileRowId);

                    if (fromTileInfo.Tile == default)
                    {
                        errorMessage = "The tile to change from cannot be found, it has likely been deleted.";
                    }
                    else
                    {
                        fromTileInfo.TaskAtSelection = fromTileInfo.Tile.Task;
                    }
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new ComponentInteractionHandlerException(errorMessage, args, false,
                        ComponentInteractionHandlerException.ErrorResponseType.CreateFollowUpResponse);
            }
        }

        private void UpdateToTileInfo(ComponentInteractionCreateEventArgs args)
        {
            if (!toTileInfo.SelectComponent.SelectedItems.Any()) { throw UpdateToTileInfoException(args, "Must select a tile to change to."); }

            if (toTileInfo.SelectComponent.SelectedItems[0].value == null) { throw UpdateToTileInfoException(args, "An internal error has occurred when retrieving the task to change to."); }

            toTileInfo.TaskAtSelection = BingoTaskRecord.GetAllBingoTasks(DataWorker)
            .FirstOrDefault(t => t.RowId == (int)toTileInfo.SelectComponent.SelectedItems[0].value!);

            if (toTileInfo.TaskAtSelection == default) { throw UpdateToTileInfoException(args, "The tile to change from cannot be found, it has likely been deleted."); }

            toTileInfo.Tile = Team!.Tiles.FirstOrDefault(t => t.TaskId == toTileInfo.TaskAtSelection.RowId);

            if (toTileInfo.Tile != default) { toTileInfo.BoardIndexAtSelection = toTileInfo.Tile.BoardIndex; }
        }

        private static Exception UpdateToTileInfoException(ComponentInteractionCreateEventArgs args, string errorMessage)
            => new ComponentInteractionHandlerException(errorMessage, args, false, ComponentInteractionHandlerException.ErrorResponseType.CreateFollowUpResponse);

        private SelectComponent CreateFromTileSelectComponent()
        {
            SelectComponent selectComponent = new(Guid.NewGuid().ToString(), "Change from",
                TileSelectComponentInteracted, TileSelectComponentInteracted);

            List<SelectComponentItem> items = new();
            IEnumerable<Tile> tiles = Team!.Tiles.OrderBy(t => t.BoardIndex);
            int tileIndex = 0;
            Tile tile;

            for (int i = 0; i < MaxTilesOnABoard; i++)
            {
                if (tileIndex < tiles.Count() && i == tiles.ElementAt(tileIndex).BoardIndex)
                {
                    tile = tiles.ElementAt(tileIndex);
                    items.Add(new(tile.Task.Name, new FromTileSelectValue(tile.RowId, i)));
                    tileIndex++;
                }
                else
                {
                    items.Add(new(NoTaskName, new FromTileSelectValue(null, i)));
                }
            }

            selectComponent.Options = new(items);
            selectComponent.Build();
            return selectComponent;
        }

        private SelectComponent CreateToTileSelectComponent()
        {
            SelectComponent selectComponent = new(Guid.NewGuid().ToString(), "Change to",
                TileSelectComponentInteracted, TileSelectComponentInteracted);

            List<Difficulty> difficulties = new();
            Dictionary<int, SelectComponentPage> pages = new();
            HashSet<int> taskIdsOnBoard = Team!.Tiles.Select(t => t.TaskId).ToHashSet();
            DiscordComponentEmoji onBoardEmoji = new("👍");

            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                difficulties.Add(difficulty);
            }

            foreach (BingoTask task in BingoTaskRecord.GetAllBingoTasks(DataWorker))
            {
                if (!pages.ContainsKey(task.Difficulty))
                {
                    Difficulty difficulty = difficulties[task.Difficulty];
                    pages.Add(task.Difficulty, new(difficulty.ToString()));
                }

                DiscordComponentEmoji? emoji = taskIdsOnBoard.Contains(task.RowId) ? onBoardEmoji : null;
                pages[task.Difficulty].Options.Add(new SelectComponentItem(task.Name, task.RowId, emoji: emoji));
            }

            for (int i = 0; i < difficulties.Count; i++)
            {
                if (pages.ContainsKey(i))
                {
                    selectComponent.Options.Add(pages[i]);
                }
            }

            if (!selectComponent.Options.Any())
            {
                throw new ComponentInteractionHandlerException("No tasks were found.", OriginalInteractionArgs, true,
                        ComponentInteractionHandlerException.ErrorResponseType.CreateFollowUpResponse);
            }
            
            selectComponent.Build();
            return selectComponent;
        }

        private async Task TileSelectComponentInteracted(ComponentInteractionCreateEventArgs args)
        {
            await UpdateMessage(args);
        }

        private async Task UpdateMessage(ComponentInteractionCreateEventArgs args)
        {
            var builder = new DiscordMessageBuilder()
                .WithContent(args.User.Mention)
                .AddComponents(fromTileInfo.SelectComponent.DiscordComponent)
                .AddComponents(toTileInfo.SelectComponent.DiscordComponent)
                .AddComponents(submitButton);

            await originalResponse.ModifyAsync(builder);
        }

        private async Task UpdateBoard()
        {
            BoardImage.UpdateTile(Team!, (int)fromTileInfo.BoardIndexAtSelection!, toTileInfo.TaskAtSelection!);

            if (toTileInfo.BoardIndexAtSelection != null)
            {
                BoardImage.UpdateTile(Team!, (int)toTileInfo.BoardIndexAtSelection, fromTileInfo.TaskAtSelection);
            }

            await RSBingoBot.DiscordTeam.UpdateBoard(Team!, BoardImage.GetTeamBoard(Team!));
        }

        private async Task SubmitButtonInteracted(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            UpdateFromTileInfo(args);
            UpdateToTileInfo(args);

            // TODO: hook up tile changes with the update handler
            string editBuilderContent = ChangeDBTiles(DataWorker, Team!, fromTileInfo.Tile, toTileInfo.TaskAtSelection!, fromTileInfo.BoardIndexAtSelection);
            DataWorker.SaveChanges();
            await UpdateBoard();

            var editBuilder = new DiscordWebhookBuilder() { }
                .WithContent(editBuilderContent);
            await args.Interaction.EditOriginalResponseAsync(editBuilder);

            await InteractionConcluded();
        }

        private class FromTileSelectValue
        {
            public int? TileRowId { get; }
            public int BoardIndex { get; }

            public FromTileSelectValue(int? tileRowId, int boardIndex)
            {
                TileRowId = tileRowId;
                BoardIndex = boardIndex;
            }
        }

        private class TileInfo
        {
            public SelectComponent SelectComponent;
            public int? BoardIndexAtSelection { get; set; } = null;
            public BingoTask? TaskAtSelection { get; set; } = null;
            public Tile? Tile { get; set; } = null;

            public TileInfo(SelectComponent selectComponent)
            {
                this.SelectComponent = selectComponent;
            }
        }
    }
}