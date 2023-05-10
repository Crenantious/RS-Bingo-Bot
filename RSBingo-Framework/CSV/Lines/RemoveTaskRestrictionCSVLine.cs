// <copyright file="RemoveTaskRestrictionCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Lines;

public class RemoveTaskRestrictionCSVLine : AddOrRemoveTaskRestrictionCSVLine
{
    public RemoveTaskRestrictionCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }
}