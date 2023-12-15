// <copyright file="RemoveTaskRestrictionsCSVHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;

internal class RemoveTaskRestrictionsCSVHandler : CSVHandler<RemoveTaskRestrictionsCSVRequest, RemoveTaskRestrictionCSVLine>
{
    private protected override void Operate(CSVData<RemoveTaskRestrictionCSVLine> data)
    {
        RemoveTaskRestrictionsCSVOperator op = new(DataWorker);
        op.Operate(data);

        foreach (string warning in op.GetWarningMessages())
        {
            AddWarning(new RemoveTaskRestrictionsCSVWarning(warning));
        }
        AddSuccess(new RemoveTaskRestrictionsCSVSuccess());
    }
}