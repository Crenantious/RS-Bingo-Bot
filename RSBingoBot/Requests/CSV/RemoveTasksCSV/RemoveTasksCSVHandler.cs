// <copyright file="RemoveTasksCSVHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.CSV;
using RSBingoBot.CSV.Lines;

internal class RemoveTasksCSVHandler : CSVHandler<RemoveTasksCSVRequest, RemoveTasksCSVLine>
{
    private protected override void Operate(CSVData<RemoveTasksCSVLine> data)
    {
        RemoveTasksCSVOperator op = new(DataWorker);
        op.Operate(data);

        foreach (string warning in op.GetWarningMessages())
        {
            AddWarning(new RemoveTasksCSVWarning(warning));
        }
        AddSuccess(new RemoveTasksCSVSuccess());
    }
}