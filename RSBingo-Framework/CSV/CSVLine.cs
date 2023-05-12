// <copyright file="CSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework.Interfaces;

public abstract class CSVLine
{
    public int LineNumber { get; }

    public CSVLine(int lineNumber, string[] values)
    {
        List<ICSVValue> CSVValues = GetValues();

        if (CSVValues.Count != values.Length)
        {
            string valuePlural = CSVValues.Count == 1 ? "" : "s";
            throw new IncorrectNumberOfCSVValuesException($"Expected {CSVValues.Count} value{valuePlural} but got {values.Length}");
        }

        LineNumber = lineNumber;
        CSVValues.ForEach(v => v.Parse(values[v.ValueIndex]));
    }

    /// <returns>A list of all <see cref="ICSVValue"/>s for this line, to be parsed.</returns>
    protected abstract List<ICSVValue> GetValues();
}