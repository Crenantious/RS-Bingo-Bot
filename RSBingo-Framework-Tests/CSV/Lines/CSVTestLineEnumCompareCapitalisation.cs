// <copyright file="CSVTestLineEnumCompareCapitalisation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingo_Framework.CSV;

public class CSVTestLineEnumCompareCapitalisation : CSVTestLineEnumBase
{
    public override CSVValueEnum<TestEnum> Value { get; } = new("Enum value", 0, true);

    public CSVTestLineEnumCompareCapitalisation(int lineNumber, string[] values) : base(lineNumber, values) { }
}