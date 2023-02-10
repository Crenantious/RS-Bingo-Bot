// <copyright file="AddTaskRestrictionCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV_reader.CSV_lines;

internal class AddTaskRestrictionCSVLine : CSVLine
{
    public string RestrictionName { get; private set; } = String.Empty;    
    public string RestrictionDescription { get; private set; } = String.Empty;

    private CSVValueGeneric<string> restrictionNameValue = new("Task restriction name", 0);
    private CSVValueGeneric<string> restrictionDescriptionValue = new("Task restriction description", 1);

    public override void Parse(string[] values)
    {
        RestrictionName = restrictionNameValue.Parse(values);
        RestrictionDescription = restrictionDescriptionValue.Parse(values);
    }
}