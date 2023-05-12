// <copyright file="RequestOperateCSVRemoveTaskRestrictions.cs" company="PlaceholderCompany">
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
/// Request for removing <see cref="Restriction"/>s via a csv file.
/// </summary>
public class RequestOperateCSVRemoveTaskRestrictions : RequestOperateCSVBase<RemoveTaskRestrictionCSVLine>
{
    /// <inheritdoc/>
    protected override string ProcessSussessResponse => "The restrictions were successfully removed from the database";

    /// <inheritdoc/>
    public RequestOperateCSVRemoveTaskRestrictions(InteractionContext ctx, IDataWorker dataWorker, DiscordAttachment attachment) :
        base(ctx, dataWorker, attachment) { }

    /// <inheritdoc/>
    private protected override IEnumerable<string> Operate()
    {
        RemoveTaskRestrictionsCSVOperator op = new(DataWorker);
        op.Operate(Data);
        return op.GetWarningMessages();
    }
}