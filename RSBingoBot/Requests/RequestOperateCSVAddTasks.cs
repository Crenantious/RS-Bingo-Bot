// <copyright file="RequestOperateCSVAddTasks.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using static RSBingo_Common.General;
using System.Linq;

/// <summary>
/// Request for adding <see cref="BingoTask"/>s via a csv file.
/// </summary>
public class RequestOperateCSVAddTasks : RequestOperateCSVBase<AddTasksCSVLine>
{
    /// <inheritdoc/>
    protected override string ProcessSussessResponse => "The tasks were successfully added to the database";

    /// <inheritdoc/>
    public RequestOperateCSVAddTasks(InteractionContext ctx, IDataWorker dataWorker, DiscordAttachment attachment) :
        base(ctx, dataWorker, attachment)
    { }

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        AddTasksCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}