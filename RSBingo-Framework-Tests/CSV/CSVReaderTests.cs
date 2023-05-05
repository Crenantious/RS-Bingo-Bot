// <copyright file="CSVReaderTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;
using static RSBingo_Framework_Tests.CSV.Lines.CSVReaderTestLine;

[TestClass]
public class CSVReaderTests : MockDBBaseTestClass
{
    private CSVData<CSVReaderTestLine> csvData;

    [TestCleanup]
    public override void TestCleanup() =>
        CSVReaderTestHelper.TestCleanup();

    [TestMethod]
    public void CreateNonCSVFile_Parse_GetException()
    {
        CreateNonCSVFile();

        Assert.ThrowsException<InvalidFileTypeException>(() => ParseFile<CSVReaderTestLine>());
    }

    [TestMethod]
    public void CreateCSVFileWithValidValuesAndNoWhiteSpace_Parse_GetCorrectValues()
    {
        CreateCSVFile($"Test,{TestEnum.TestValue},1");

        csvData = ParseCSVFile<CSVReaderTestLine>();

        AssertParsedValues("Test", TestEnum.TestValue, 1);
    }

    [TestMethod]
    public void CreateCSVFileWithValidValuesAndDifferentAmountsOfWhiteSpace_Parse_GetCorrectValues()
    {
        CreateCSVFile($"Test,              {TestEnum.TestValue},1");

        csvData = ParseCSVFile<CSVReaderTestLine>();

        AssertParsedValues("Test", TestEnum.TestValue, 1);
    }

    [TestMethod]
    public void CreateCSVFileWithValidValuesInAnIncorrectOrder_Parse_GetException()
    {
        CreateCSVFile($"Test, 1, {TestEnum.TestValue}");

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile<CSVReaderTestLine>());
    }

    [TestMethod]
    public void CreateCSVFileWithTooFewValues_Parse_GetException()
    {
        CreateCSVFile("1");

        Assert.ThrowsException<IncorrectNumberOfCSVValuesException>(() => ParseCSVFile<CSVReaderTestLine>());
    }

    [TestMethod]
    public void CreateCSVFileWithTooManyValues_Parse_GetException()
    {
        CreateCSVFile("1, 2, 3, 4");

        Assert.ThrowsException<IncorrectNumberOfCSVValuesException>(() => ParseCSVFile<CSVReaderTestLine>());
    }

    private void AssertParsedValues(string genericValue, TestEnum enumValue, int comparableValue)
    {
        CSVReaderTestLine line = csvData.Lines.ElementAt(0);

        Assert.AreEqual(genericValue, line.GenericValue.Value);
        Assert.AreEqual(enumValue, line.EnumValue.Value);
        Assert.AreEqual(comparableValue, line.CompareableValue.Value);
    }
}