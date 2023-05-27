// <copyright file="RequestOperateCSVAddTasks.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Models;
using RSBingo_Framework.CSV.Lines;
using DSharpPlus.Entities;

/// <summary>
/// Request for adding <see cref="BingoTask"/>s via a csv file.
/// </summary>
internal class RequestOperateCSVAddTasks : RequestOperateCSVBase<AddTasksCSVLine>
{
    /// <inheritdoc/>
    protected override string ProcessSussessResponse => "The tasks were successfully added to the database";

    /// <inheritdoc/>
    public RequestOperateCSVAddTasks(DiscordAttachment attachment) : base(attachment) { }

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        AddTasksCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}