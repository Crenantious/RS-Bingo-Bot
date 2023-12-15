// <copyright file="CSVData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.Exceptions.CSV;

public class CSVData<CSVLineType> where CSVLineType : CSVLine
{
    public IEnumerable<CSVLineType> Lines { get; }

    public CSVData(IEnumerable<CSVLineType> lines) =>
        Lines = lines;
}