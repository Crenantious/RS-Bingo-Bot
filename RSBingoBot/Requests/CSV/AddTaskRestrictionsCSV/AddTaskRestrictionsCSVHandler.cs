// <copyright file="AddTaskRestrictionsCSVHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;

internal class AddTaskRestrictionsCSVHandler : CSVHandler<AddTaskRestrictionsCSVRequest, AddTaskRestrictionCSVLine>
{
    private protected override void Operate(CSVData<AddTaskRestrictionCSVLine> data)
    {
        AddTaskRestrictionsCSVOperator op = new(DataWorker);
        op.Operate(data);

        foreach (string warning in op.GetWarningMessages())
        {
            AddWarning(new AddTaskRestrictionsCSVWarning(warning));
        }
        AddSuccess(new AddTaskRestrictionsCSVSuccess());
    }
}