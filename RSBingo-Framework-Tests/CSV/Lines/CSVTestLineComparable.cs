// <copyright file="CSVTestLineComparable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingo_Framework.CSV;

public class CSVTestLineComparable : CSVLine
{
    public const int ComparableValueMin = 1;
    public const int ComparableValueMax = 5;

    private CSVValueComparable<int> comparableValue = new("Comparable value", 0, ComparableValueMin, ComparableValueMax);

    public int ComparableValue { get; private set; }

    protected override int NumberOfValues => 1;

    public CSVTestLineComparable(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override void Parse(string[] values)
    {
        ComparableValue = comparableValue.Parse(values);
    }
}