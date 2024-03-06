// <copyright file="ChangeTilesTaskSelect.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Factories;
using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

public class ChangeTilesTaskSelect
{
    private const string NoTaskName = "No task";

    private readonly DiscordEmoji taskOnBoardEmoji;
    private readonly Team team;

    private Dictionary<int, SelectComponentItem> taskIdToItem = new();

    public SelectComponent SelectComponent { get; }

    public ChangeTilesTaskSelect(IDataWorker dataWorker, Team team, ChangeTilesButtonDTO dto)
    {
        SelectComponentFactory selectComponentFactory = (SelectComponentFactory)General.DI.GetService(typeof(SelectComponentFactory))!;
        DiscordClient client = (DiscordClient)General.DI.GetService(typeof(DiscordClient))!;

        this.team = team;
        taskOnBoardEmoji = DiscordEmoji.FromUnicode(client, "✅");
        SelectComponent = CreateSelectComponent(dataWorker, dto, selectComponentFactory);
    }

    public void Update(IEnumerable<BingoTask> updatedTasks)
    {
        UpdateItem(SelectComponent.SelectedItems.ElementAt(0));

        foreach (BingoTask task in updatedTasks)
        {
            UpdateItem(taskIdToItem[task.RowId]);
        }

        SelectComponent.Build();
    }

    public void UpdateItem(SelectComponentItem item)
    {
        item.Emoji = GetEmoji((BingoTask)item.Value!);
        SelectComponent.Build();
    }

    private SelectComponent CreateSelectComponent(IDataWorker dataWorker, ChangeTilesButtonDTO dto, SelectComponentFactory selectComponentFactory) =>
        selectComponentFactory.Create(new(new SelectComponentPage("Change to", GetOptions(dataWorker),
            SelectComponentGetPageName.CustomMethod(GetPageName))), () => new ChangeTilesTaskSelectRequest(dto));

    private IEnumerable<SelectComponentOption> GetOptions(IDataWorker dataWorker)
    {
        IEnumerable<BingoTask> tasks = dataWorker.BingoTasks.GetAll();
        SelectComponentPage easy = new("Easy", GetToSelectItems(tasks, BingoTaskRecord.Difficulty.Easy));
        SelectComponentPage medium = new("Medium", GetToSelectItems(tasks, BingoTaskRecord.Difficulty.Medium));
        SelectComponentPage hard = new("Hard", GetToSelectItems(tasks, BingoTaskRecord.Difficulty.Hard));
        return new List<SelectComponentOption> { easy, medium, hard };
    }

    private List<SelectComponentOption> GetToSelectItems(IEnumerable<BingoTask> tasks, BingoTaskRecord.Difficulty difficulty)
    {
        var items = tasks.Where(t => t.GetDifficutyAsDifficulty() == difficulty)
               .Select(t => CreateItem(t))
               .ToList();

        if (items.Count == 0)
        {
            items.Add(new SelectComponentItem("No tasks found", null));
        }

        return items;
    }

    private SelectComponentOption CreateItem(BingoTask task)
    {
        var item = new SelectComponentItem(task.Name, task, emoji: GetEmoji(task));
        taskIdToItem.Add(task.RowId, item);
        return item;
    }

    private DiscordComponentEmoji? GetEmoji(BingoTask task) =>
        IsTaskOnTheBoard(task) ? new(taskOnBoardEmoji) : null;

    private bool IsTaskOnTheBoard(BingoTask task) =>
        team.Tiles.FirstOrDefault(t => t.TaskId == task.RowId) != default;

    private string GetPageName(SelectComponentPage page)
    {
        var firstPage = (SelectComponentItem)page.Options[0];
        var lastPage = (SelectComponentItem)page.Options[^1];
        return $"Tiles {firstPage.Value} - {lastPage.Value}";
    }
}