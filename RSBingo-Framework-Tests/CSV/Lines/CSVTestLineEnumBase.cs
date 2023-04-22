// <copyright file="CSVTestLineEnumBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingo_Framework.CSV;

public abstract class CSVTestLineEnumBase : CSVLine
{
    public abstract CSVValueEnum<TestEnum> Value { get; }

    public enum TestEnum
    {
        TestValue
    }

    public CSVTestLineEnumBase(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override List<ICSVValue> GetValues() =>
        new() { Value };
}