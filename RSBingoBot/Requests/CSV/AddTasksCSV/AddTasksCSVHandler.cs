// <copyright file="AddTasksCSVHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;
using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;

internal class AddTasksCSVHandler : CSVHandler<AddTasksCSVRequest, AddTasksCSVLine>
{
    private protected override void Operate(CSVData<AddTasksCSVLine> data)
    {
        AddTasksCSVOperator op = new(DataWorker);
        op.Operate(data);

        foreach (string warning in op.GetWarningMessages())
        {
            AddWarning(new AddTasksCSVWarning(warning));
        }
        AddSuccess(new AddTasksCSVSuccess());
    }
}