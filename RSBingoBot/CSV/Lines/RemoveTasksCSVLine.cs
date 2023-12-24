// <copyright file="RemoveTasksCSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Lines;

public class RemoveTasksCSVLine : AddOrRemoveTasksCSVLine
{
    public RemoveTasksCSVLine(int lineNumber, string[] values) : base(lineNumber, values) { }
}