// <copyright file="TaskRestrictionDoesNotExistWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Operators.Warnings;

public class TaskRestrictionDoesNotExistWarning : Warning
{
    public override string Message => "The task restriction does not exist";
    public TaskRestrictionDoesNotExistWarning(int valueIndex, int lineNumber) : base(valueIndex, lineNumber) { }
}