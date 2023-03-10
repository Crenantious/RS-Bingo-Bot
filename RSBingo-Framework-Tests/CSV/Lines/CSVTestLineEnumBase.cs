// <copyright file="CSVTestLineEnumBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingo_Framework.CSV;

public abstract class CSVTestLineEnumBase : CSVLine
{
    public TestEnum EnumValue { get; private set; }

    protected abstract CSVValueEnum<TestEnum> enumValue { get; }

    protected override int NumberOfValues => 1;

    public enum TestEnum
    {
        TestValue
    }

    public CSVTestLineEnumBase(int lineNumber, string[] values)
        : base(lineNumber, values) { }

    protected override void Parse(string[] values) =>
        EnumValue = enumValue.Parse(values);
}