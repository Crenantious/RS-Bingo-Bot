// <copyright file="ChangeTilesButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DSharpPlus;
using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;

// TODO: JR - only show the "to" select when an item has been selected in the "from" select. When a new value gets selected from "from",
// update "to" such that only tasks of the allowed difficulty are options.
internal class ChangeTilesButtonHandler : ButtonHandler<ChangeTilesButtonRequest>
{
    private const string NoTaskName = "No task";
    private const string ResponseContent = "{0} select tiles to swap. The 'from' selection contains the team's tiles," +
        "the 'to' selection contains all possible tiles. A tick in the 'to' selection indicates the tile is on the board.";

    private readonly ButtonFactory buttonFactory;
    private readonly SelectComponentFactory selectFactory;
    private readonly IDiscordMessageServices messageServices;
    private readonly DiscordClient client;
    private readonly DiscordEmoji taskOnBoardEmoji;

    private User user = null!;

    public ChangeTilesButtonHandler(ButtonFactory buttonFactory, SelectComponentFactory selectFactory, IDiscordMessageServices messageServices,
        DiscordClient client)
    {
        this.buttonFactory = buttonFactory;
        this.selectFactory = selectFactory;
        this.messageServices = messageServices;
        this.client = client;
        taskOnBoardEmoji = DiscordEmoji.FromUnicode(client, "✅");
    }

    protected override async Task Process(ChangeTilesButtonRequest request, CancellationToken cancellationToken)
    {
        user = GetUser()!;

        var response = new InteractionMessage(InteractionArgs.Interaction)
            .WithContent(GetResponseContent(request))
            .AsEphemeral(true);
        ChangeTilesButtonDTO dto = new();

        SelectComponent changeFrom = CreateSelectComponent("Change from", GetFromSelectOptions(), new ChangeTilesFromSelectRequest(dto));
        SelectComponent changeTo = CreateSelectComponent("Change to", GetToSelectOptions(), new ChangeTilesToSelectRequest(dto));

        Button submit = buttonFactory.Create(new(ButtonStyle.Primary, "Submit"), new ChangeTilesSubmitButtonRequest(user.Team.RowId, dto));
        Button cancel = buttonFactory.Create(new(ButtonStyle.Primary, "Cancel"), new ConcludeInteractionButtonRequest(this));

        response.AddComponents(changeFrom);
        response.AddComponents(changeTo);
        response.AddComponents(submit, cancel);
        ResponseMessages.Add(response);
    }

    public override Task Conclude()
    {
        DeleteResponses();
        return base.Conclude();
    }

    private static string GetResponseContent(ChangeTilesButtonRequest request) =>
        ResponseContent.FormatConst(request.InteractionArgs.User.Mention);

    private SelectComponent CreateSelectComponent(string name, IEnumerable<SelectComponentOption> options, ISelectComponentRequest request) =>
        selectFactory.Create(new SelectComponentInfo(name, options), request);

    private IEnumerable<SelectComponentOption> GetFromSelectOptions()
    {
        List<SelectComponentItem> items = new();
        IEnumerator<Tile> tiles = user.Team.Tiles.OrderBy(t => t.BoardIndex).GetEnumerator();

        for (int boardIndex = 0; boardIndex < General.MaxTilesOnABoard; boardIndex++)
        {
            if (boardIndex == tiles.Current.BoardIndex)
            {
                items.Add(new(tiles.Current.Task.Name, boardIndex));
                tiles.MoveNext();
            }
            else
            {
                items.Add(new(NoTaskName, boardIndex));
            }
        }

        return items;
    }

    private IEnumerable<SelectComponentOption> GetToSelectOptions()
    {
        IEnumerable<BingoTask> tasks = DataWorker.BingoTasks.GetAll();
        SelectComponentPage easy = new("Easy");
        SelectComponentPage medium = new("Medium");
        SelectComponentPage hard = new("Hard");
        easy.Options = GetToSelectItems(tasks, BingoTaskRecord.Difficulty.Easy);
        medium.Options = GetToSelectItems(tasks, BingoTaskRecord.Difficulty.Medium);
        hard.Options = GetToSelectItems(tasks, BingoTaskRecord.Difficulty.Hard);

        return new List<SelectComponentOption> { easy, medium, hard };
    }

    private List<SelectComponentOption> GetToSelectItems(IEnumerable<BingoTask> tasks, BingoTaskRecord.Difficulty difficulty) =>
        tasks.Where(t => t.GetDifficutyAsDifficulty() == difficulty)
            .Select(t => CreateToSelectItem(t))
            .ToList();

    private SelectComponentOption CreateToSelectItem(BingoTask task)
    {
        DiscordComponentEmoji? emoji = IsTaskOnTheBoard(task) ? new(taskOnBoardEmoji) : null;
        return new SelectComponentItem(task.Name, task, emoji: emoji);
    }

    private bool IsTaskOnTheBoard(BingoTask task) =>
        user.Team.Tiles.FirstOrDefault(t => t.TaskId == task.RowId) != default;
}