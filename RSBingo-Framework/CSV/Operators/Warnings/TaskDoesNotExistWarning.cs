// <copyright file="TaskDoesNotExistWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Operators.Warnings;

public class TaskDoesNotExistWarning : Warning
{
    public override string Message => "The task does not exist";
    public TaskDoesNotExistWarning(int valueIndex, int lineNumber) : base(valueIndex, lineNumber) { }
}