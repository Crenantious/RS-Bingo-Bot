// <copyright file="CSVReaderTestsHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;

public class CSVReaderTestHelper : MockDBBaseTestClass
{
    private const string CSVFileName = "Test.csv";
    private const string FileName = "Test.txt";

    private static CSVReader reader = new CSVReader();

    public static CSVData<LineType> CreateAndParseCSVFile<LineType>(params string[] lines)
        where LineType : CSVLine
    {
        CreateCSVFile(lines);
        return ParseCSVFile<LineType>();
    }

    public static CSVData<LineType> CreateAndParseFile<LineType>()
        where LineType : CSVLine
    {
        CreateNonCSVFile();
        return ParseFile<LineType>();
    }

    public static void CreateCSVFile(params string[] lines) =>
        File.WriteAllLines(CSVFileName, lines);

    public static void CreateNonCSVFile() =>
        File.Create(FileName);

    public static CSVData<LineType> ParseCSVFile<LineType>() where LineType : CSVLine =>
        reader.Parse<LineType>(CSVFileName);

    public static CSVData<LineType> ParseFile<LineType>() where LineType : CSVLine =>
        reader.Parse<LineType>(FileName);

    public static void TestCleanup()
    {
        try { File.Delete(CSVFileName); }
        catch { }
        try { File.Delete(FileName); }
        catch { }
    }
}