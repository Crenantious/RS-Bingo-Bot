// <copyright file="CSVReaderTestLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingoBot.CSV;

public class CSVReaderTestLine : CSVLine
{
    public const int CompareableValueMin = 1;
    public const int CompareableValueMax = 5;

    public CSVValueGeneric<string> GenericValue { get; } = new("Generic value", 0);
    public CSVValueEnum<TestEnum> EnumValue { get; } = new("Enum value", 1);
    public CSVValueComparable<int> CompareableValue { get; } = new("CompareableValue", 2, CompareableValueMin, CompareableValueMax);

    public enum TestEnum
    {
        TestValue
    }

    ///<inheritdoc/>
    public CSVReaderTestLine(int lineNumber, string[] values) : base(lineNumber, values) { }

    ///<inheritdoc/>
    protected override List<ICSVValue> GetValues() =>
        new() { GenericValue, EnumValue, CompareableValue };
}