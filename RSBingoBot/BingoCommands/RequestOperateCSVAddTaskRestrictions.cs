// <copyright file="RequestOperateCSVAddTaskRestrictions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using static RSBingo_Common.General;

/// <summary>
/// Request for adding <see cref="Restriction"/>s via a csv file.
/// </summary>
public class RequestOperateCSVAddTaskRestrictions : RequestOperateCSVBase<AddTaskRestrictionCSVLine>
{
    /// <inheritdoc/>
    protected override string ProcessSussessResponse => "The restrictions were successfully added to the database";

    /// <inheritdoc/>
    public RequestOperateCSVAddTaskRestrictions(InteractionContext ctx, IDataWorker dataWorker, DiscordAttachment attachment) :
        base(ctx, dataWorker, attachment) { }

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        AddTaskRestrictionsCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}