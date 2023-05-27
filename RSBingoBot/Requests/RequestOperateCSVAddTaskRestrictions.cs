// <copyright file="RequestOperateCSVAddTaskRestrictions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Models;
using RSBingo_Framework.CSV.Lines;
using DSharpPlus.Entities;

/// <summary>
/// Request for adding <see cref="Restriction"/>s via a csv file.
/// </summary>
internal class RequestOperateCSVAddTaskRestrictions : RequestOperateCSVBase<AddTaskRestrictionCSVLine>
{
    /// <inheritdoc/>
    protected override string ProcessSussessResponse => "The restrictions were successfully added to the database";

    /// <inheritdoc/>
    public RequestOperateCSVAddTaskRestrictions(DiscordAttachment attachment) : base(attachment) { }

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        AddTaskRestrictionsCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}