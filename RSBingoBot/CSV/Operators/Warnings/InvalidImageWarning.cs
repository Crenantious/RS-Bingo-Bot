// <copyright file="InvalidImageWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Operators.Warnings;

public class InvalidImageWarning : Warning
{
    public override string Message => "Invalid image";
    public InvalidImageWarning(int valueIndex, int lineNumber) : base(valueIndex, lineNumber) { }
}