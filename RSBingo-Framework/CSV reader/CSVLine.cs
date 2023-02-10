// <copyright file="CSVLine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.Exceptions;

namespace RSBingo_Framework.CSV_reader;

internal abstract class CSVLine
{
    public abstract void Parse(string[] values);
}