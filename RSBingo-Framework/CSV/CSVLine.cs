// <copyright file="CSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Exceptions;

namespace RSBingo_Framework.CSV;

public abstract class CSVLine
{
    public int LineNumber { get; }

    public CSVLine(int lineNumber, string[] values)
    {
        LineNumber = lineNumber;
        Parse(values);
    }

    protected abstract void Parse(string[] values);
}