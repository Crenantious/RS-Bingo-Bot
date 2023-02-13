// <copyright file="CSVData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

public class CSVData<LineType> where LineType : CSVLine
{
    public IEnumerable<LineType> Lines { get; }

    public CSVData(IEnumerable<LineType> Lines) =>
        this.Lines = Lines;
}