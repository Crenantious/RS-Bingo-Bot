// <copyright file="RemoveTaskRestrictionCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV_reader.CSV_lines;

internal class RemoveTaskRestrictionCSVLine : CSVLine
{
    public string RestrictionName { get; private set; } = String.Empty;    

    private CSVValueGeneric<string> restrictionNameValue = new("Task restriction name", 0);

    public override void Parse(string[] values)
    {
        RestrictionName = restrictionNameValue.Parse(values);
    }
}