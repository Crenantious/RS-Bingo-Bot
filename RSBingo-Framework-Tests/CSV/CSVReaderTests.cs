// <copyright file="CSVReaderTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.DTO;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;
using static RSBingo_Framework_Tests.CSV.Lines.CSVReaderTestLine;

[TestClass]
public class CSVReaderTests : MockDBBaseTestClass
{
    private ReaderResults<CSVReaderTestLine> readerResults = null!;

    [TestMethod]
    public void CreateNonCSVFile_Parse_GetException()
    {
        CreateAndParseFile("Test.txt");

        AssertReader(typeof(InvalidFileTypeException));
    }

    [TestMethod]
    public void CreateCSVFileWithValidValuesAndNoWhiteSpace_Parse_GetCorrectValuesAndNoExceptions()
    {
        CreateAndParseCSVFile($"Test,{TestEnum.TestValue},1");

        AssertReader(null);
        AssertParsedValues("Test", TestEnum.TestValue, 1);
    }

    [TestMethod]
    public void CreateCSVFileWithValidValuesAndDifferentAmountsOfWhiteSpace_Parse_GetCorrectValuesAndNoExceptions()
    {
        CreateAndParseCSVFile($"Test,              {TestEnum.TestValue},1");

        AssertReader(null);
        AssertParsedValues("Test", TestEnum.TestValue, 1);
    }

    [TestMethod]
    public void CreateCSVFileWithValidValuesInIncorrectOrder_Parse_GetException()
    {
        CreateAndParseCSVFile($"Test, 1, {TestEnum.TestValue}");

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    [TestMethod]
    public void CreateCSVFileWithTooFewValues_Parse_GetException()
    {
        CreateAndParseCSVFile("1");

        AssertReader(typeof(IncorrectNumberOfCSVValuesException));
    }

    [TestMethod]
    public void CreateCSVFileWithTooManyValues_Parse_GetException()
    {
        CreateAndParseCSVFile("1, 2, 3, 4");

        AssertReader(typeof(IncorrectNumberOfCSVValuesException));
    }

    private void CreateAndParseCSVFile(params string[] lines) =>
        readerResults = CreateAndParseCSVFile<CSVReaderTestLine>(lines);

    private void CreateAndParseFile(string fileName) =>
        readerResults = CreateAndParseFile<CSVReaderTestLine>(fileName);

    private void AssertReader(Type? exceptionType) =>
        Assert.AreEqual(exceptionType, readerResults.exceptionType);

    private void AssertParsedValues(string genericValue, TestEnum enumValue, int comparableValue)
    {
        CSVReaderTestLine line = readerResults.data.Lines.ElementAt(0);

        Assert.AreEqual(genericValue, line.GenericValue.Value);
        Assert.AreEqual(enumValue, line.EnumValue.Value);
        Assert.AreEqual(comparableValue, line.CompareableValue.Value);
    }
}