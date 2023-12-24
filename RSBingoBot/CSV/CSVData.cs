// <copyright file="CSVData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.CSV;

public class CSVData<CSVLineType> where CSVLineType : CSVLine
{
    public IEnumerable<CSVLineType> Lines { get; }

    public CSVData(IEnumerable<CSVLineType> lines) =>
        Lines = lines;
}