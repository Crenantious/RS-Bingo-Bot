// <copyright file="CSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Exceptions.CSV;

public abstract class CSVLine
{
    public int LineNumber { get; }

    protected abstract int NumberOfValues { get; }

    public CSVLine(int lineNumber, string[] values)
    {
        if(values.Length != NumberOfValues)
        {
            throw new IncorrectNumberOfCSVValuesException($"Expected {NumberOfValues} values but got {values.Length}");
        }

        LineNumber = lineNumber;
        Parse(values);
    }

    protected abstract void Parse(string[] values);
}