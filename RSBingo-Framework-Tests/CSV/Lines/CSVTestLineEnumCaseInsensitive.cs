// <copyright file="CSVTestLineEnumCaseInsensitive.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingoBot.CSV;

public class CSVTestLineEnumCaseInsensitive : CSVTestLineEnumBase
{
    public override CSVValueEnum<TestEnum> Value { get; } = new("Enum value", 0);

    public CSVTestLineEnumCaseInsensitive(int lineNumber, string[] values) : base(lineNumber, values) { }
}