// <copyright file="NotEnoughTasksExistWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV.Operators.Warnings;

public class NotEnoughTasksToDeleteWarning : Warning
{
    public override string Message { get; }

    public NotEnoughTasksToDeleteWarning(int valueIndex, int lineNumber, int amoutFound) : base(valueIndex, lineNumber) =>
        Message = $"Not enough tasks exist. Could only deleted {amoutFound}";
}