// <copyright file="ChangeTilesButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

// TODO: JR - only show the "to" select when an item has been selected in the "from" select. When a new value gets selected from "from",
// update "to" such that only tasks of the allowed difficulty are options.
internal class ChangeTilesButtonHandler : ButtonHandler<ChangeTilesButtonRequest>
{
    private const string NoTaskName = "No task";
    private const string ResponseContent = "{0} Select tiles to swap. The 'Change from' selection contains the team's tiles, " +
        "the 'Change to' selection contains all possible tiles. A tick in the 'Change to' selection indicates the tile is on the board.";

    private readonly ButtonFactory buttonFactory;
    private readonly SelectComponentFactory selectFactory;
    private readonly DiscordClient client;
    private readonly DiscordEmoji taskOnBoardEmoji;

    private User user = null!;

    protected override bool SendKeepAliveMessage => false;

    public ChangeTilesButtonHandler(ButtonFactory buttonFactory, SelectComponentFactory selectFactory, DiscordClient client)
    {
        this.buttonFactory = buttonFactory;
        this.selectFactory = selectFactory;
        this.client = client;
        taskOnBoardEmoji = DiscordEmoji.FromUnicode(client, "✅");
    }

    protected override async Task Process(ChangeTilesButtonRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordInteractionMessagingServices>();
        await messageServices.SendKeepAlive(Interaction, false);

        user = GetUser()!;

        var response = new InteractionMessage(Interaction)
            .WithContent(GetResponseContent(request));

        ChangeTilesButtonDTO dto = new();

        SelectComponent changeFrom = CreateSelectComponent("Change from", GetFromSelectOptions(), () => new ChangeTilesFromSelectRequest(dto),
            SelectComponentGetPageName.CustomMethod(GetFromSelectPageName));
        SelectComponent changeTo = CreateSelectComponent("Change to", GetToSelectOptions(), () => new ChangeTilesToSelectRequest(dto),
            SelectComponentGetPageName.FirstToLastOptions());

        Button changeFromBack = buttonFactory.CreateSelectComponentBackButton(() => new(changeFrom));
        Button changeToBack = buttonFactory.CreateSelectComponentBackButton(() => new(changeTo));
        Button apply = buttonFactory.Create(new(ButtonStyle.Primary, "Apply"),
            () => new ChangeTilesSubmitButtonRequest(user.Team.RowId, dto, Interaction.User));
        Button close = buttonFactory.CreateConcludeInteraction(() => new(InteractionTracker, new List<Message>() { response }));

        response.AddComponents(changeFrom)
            .AddComponents(changeFromBack)
            .AddComponents(changeTo)
            .AddComponents(changeToBack)
            .AddComponents(apply, close);

        await messageServices.Send(response);
    }

    private static string GetResponseContent(ChangeTilesButtonRequest request) =>
        ResponseContent.FormatConst(request.GetDiscordInteraction().User.Mention);

    private SelectComponent CreateSelectComponent(string name, IEnumerable<SelectComponentOption> options, Func<ISelectComponentRequest> request,
        SelectComponentGetPageName getPageName) =>
        selectFactory.Create(new(new SelectComponentPage(name, options, getPageName)), request);

    private IEnumerable<SelectComponentOption> GetFromSelectOptions()
    {
        List<SelectComponentItem> items = new();
        Dictionary<int, Tile> tiles = user.Team.Tiles.ToDictionary(t => t.BoardIndex);

        for (int boardIndex = 0; boardIndex < General.MaxTilesOnABoard; boardIndex++)
        {
            string name = tiles.ContainsKey(boardIndex) ? tiles[boardIndex].Task.Name : NoTaskName;
            items.Add(new($"Tile {boardIndex} - {name}", boardIndex));
        }

        return items;
    }

    private string GetFromSelectPageName(SelectComponentPage page)
    {
        var firstPage = (SelectComponentItem)page.Options[0];
        var lastPage = (SelectComponentItem)page.Options[^1];
        return $"Tiles {firstPage.Value} - {lastPage.Value}";
    }

    private IEnumerable<SelectComponentOption> GetToSelectOptions()
    {
        IEnumerable<BingoTask> tasks = DataWorker.BingoTasks.GetAll();
        SelectComponentPage easy = new("Easy", GetToSelectItems(tasks, BingoTaskRecord.Difficulty.Easy));
        SelectComponentPage medium = new("Medium", GetToSelectItems(tasks, BingoTaskRecord.Difficulty.Medium));
        SelectComponentPage hard = new("Hard", GetToSelectItems(tasks, BingoTaskRecord.Difficulty.Hard));
        return new List<SelectComponentOption> { easy, medium, hard };
    }

    private List<SelectComponentOption> GetToSelectItems(IEnumerable<BingoTask> tasks, BingoTaskRecord.Difficulty difficulty)
    {
        var items = tasks.Where(t => t.GetDifficutyAsDifficulty() == difficulty)
               .Select(t => CreateToSelectItem(t))
               .ToList();

        if (items.Count == 0)
        {
            items.Add(new SelectComponentItem("No tasks found", null));
        }

        return items;
    }

    private SelectComponentOption CreateToSelectItem(BingoTask task)
    {
        DiscordComponentEmoji? emoji = IsTaskOnTheBoard(task) ? new(taskOnBoardEmoji) : null;
        return new SelectComponentItem(task.Name, task, emoji: emoji);
    }

    private bool IsTaskOnTheBoard(BingoTask task) =>
        user.Team.Tiles.FirstOrDefault(t => t.TaskId == task.RowId) != default;
}