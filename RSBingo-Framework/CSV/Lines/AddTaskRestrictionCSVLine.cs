﻿// <copyright file="AddTaskRestrictionCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Interfaces;

namespace RSBingo_Framework.CSV.Lines;

public class AddTaskRestrictionCSVLine : AddOrRemoveTaskRestrictionCSVLine
{
    public CSVValueGeneric<string> RestrictionDescription { get; } = new("Task restriction description", 1);

    public AddTaskRestrictionCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override List<ICSVValue> GetValues() =>
        new List<ICSVValue>(base.GetValues()) { RestrictionDescription };
}