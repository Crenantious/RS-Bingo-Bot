// <copyright file="CSVTestLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingo_Framework.CSV;

public class CSVReaderTestLine : CSVLine
{
    public const int CompareableValueMin = 1;
    public const int CompareableValueMax = 5;

    private CSVValueGeneric<string> genericValue = new("Generic value", 0);
    private CSVValueEnum<TestEnum> enumValue = new("Enum value", 1, false);
    private CSVValueComparable<int> compareableValue = new("CompareableValue", 2, CompareableValueMin, CompareableValueMax);

    public string GenericValue { get; private set; } = string.Empty;
    public TestEnum EnumValue { get; private set; }
    public int CompareableValue { get; private set; }

    protected override int NumberOfValues => 3;

    public enum TestEnum
    {
        TestValue
    }

    public CSVReaderTestLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override void Parse(string[] values)
    {
        GenericValue = genericValue.Parse(values);
        EnumValue = enumValue.Parse(values);
        CompareableValue = compareableValue.Parse(values);
    }
}