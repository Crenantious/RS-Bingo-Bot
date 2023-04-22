// <copyright file="CSVTestLineComparable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingo_Framework.CSV;

public class CSVTestLineComparable : CSVLine
{
    public const int ComparableValueMin = 1;
    public const int ComparableValueMax = 5;

    public CSVValueComparable<int> Value { get; } = new("Comparable value", 0, ComparableValueMin, ComparableValueMax);

    public CSVTestLineComparable(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override List<ICSVValue> GetValues() =>
        new() { Value };
}