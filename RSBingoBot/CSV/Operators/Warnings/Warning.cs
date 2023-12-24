// <copyright file="Warning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV.Operators.Warnings;

public abstract class Warning
{
    public abstract string Message { get; }
    public int ValueIndex { get; }
    public int LineNumber { get; }

    public Warning(int valueIndex, int lineNumber)
    {
        ValueIndex = valueIndex;
        LineNumber = lineNumber;
    }
}