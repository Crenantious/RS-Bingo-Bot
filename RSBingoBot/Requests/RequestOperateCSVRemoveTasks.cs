// <copyright file="RequestOperateCSVRemoveTasks.cs" company="PlaceholderCompany">
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

/// <summary>
/// Request for removing <see cref="BingoTask"/>s via a csv file.
/// </summary>
public class RequestOperateCSVRemoveTasks : RequestOperateCSVBase<RemoveTasksCSVLine>
{
    /// <inheritdoc/>
    protected override string ProcessSussessResponse => "The tasks were successfully removed from the database";

    /// <inheritdoc/>
    public RequestOperateCSVRemoveTasks(InteractionContext ctx, IDataWorker dataWorker, DiscordAttachment attachment) :
        base(ctx, dataWorker, attachment)
    { }

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        RemoveTasksCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}