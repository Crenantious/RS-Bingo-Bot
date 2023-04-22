// <copyright file="CSVTestLineGeneric.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingo_Framework.CSV;
using System.Collections.Generic;

public class CSVTestLineGeneric : CSVLine
{
    public CSVValueGeneric<int> Value { get; } = new("Generic value", 0);

    public CSVTestLineGeneric(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override List<ICSVValue> GetValues() =>
        new() { Value };
}