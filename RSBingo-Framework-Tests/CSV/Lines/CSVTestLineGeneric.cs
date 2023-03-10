// <copyright file="CSVTestLineGeneric.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.Lines;

using RSBingo_Framework.CSV;

public class CSVTestLineGeneric : CSVLine
{
    private CSVValueGeneric<int> genericValue = new("Generic value", 0);

    public int GenericValue { get; private set; }

    protected override int NumberOfValues => 1;

    public CSVTestLineGeneric(int lineNumber, string[] values) : base(lineNumber, values) { }

    protected override void Parse(string[] values)
    {
        GenericValue = genericValue.Parse(values);
    }
}