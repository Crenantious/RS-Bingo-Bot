// <copyright file="CSVReaderTestsHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;

public class CSVReaderTestHelper : MockDBBaseTestClass
{
    private const string CSVFileName = "{0}Test.csv";

    private static CSVReader reader = new CSVReader();

    static CSVReaderTestHelper() =>
        string.Format(CSVFileName, Path.GetTempPath());

    public static ReaderResults<LineType> CreateAndParseCSVFile<LineType>(params string[] lines)
        where LineType : CSVLine
    {
        CreateCSVFile(lines);
        return ParseFile<LineType>(CSVFileName);
    }

    public static ReaderResults<LineType> CreateAndParseFile<LineType>(string fileName)
        where LineType : CSVLine
    {
        File.Create(fileName);
        return ParseFile<LineType>(fileName);
    }

    public void Cleanup() =>
        File.Delete(CSVFileName);

    private static void CreateCSVFile(params string[] lines) =>
        File.WriteAllLines(CSVFileName, lines);

    private static ReaderResults<LineType> ParseFile<LineType>(string fileName)
        where LineType : CSVLine
    {
        CSVData<LineType>? ParsedCSVData = null;
        Type? exceptionType = null;

        try { ParsedCSVData = reader.Parse<LineType>(fileName); }
        catch (CSVReaderException e) { exceptionType = e.GetType(); }

        return new ReaderResults<LineType>(ParsedCSVData, exceptionType);
    }
}