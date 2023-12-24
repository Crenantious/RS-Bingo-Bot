// <copyright file="TaskRestrictionAlreadyExistsWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Operators.Warnings;

public class TaskRestrictionAlreadyExistsWarning : Warning
{
    public override string Message => "The task restriction already exists";
    public TaskRestrictionAlreadyExistsWarning(int valueIndex, int lineNumber) : base(valueIndex, lineNumber) { }
}