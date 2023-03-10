// <copyright file="CSVReaderTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework.Models;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.Lines.CSVReaderTestLine;

[TestClass]
public class CSVReaderTests : CSVTestsBase<CSVReaderTestLine>
{
    [TestMethod]
    public void CreateNonCSVFile_Parse_GetFormatException()
    {
        CreateAndParseFile("Test.txt");

        AssertReader(typeof(InvalidFileTypeException));
    }

    [TestMethod]
    public void CreateCSVFileWithValidValuesAndNoWhiteSpace_Parse_GetCorrectValuesAndNoExceptions()
    {
        CreateAndParseCSVFile($"Test,{TestEnum.TestValue},1");

        AssertParsedValues("Test", TestEnum.TestValue, 1);
    }

    [TestMethod]
    public void CreateCSVFileWithValidValuesAndDifferentAmountsOfWhiteSpace_Parse_GetCorrectValuesAndNoExceptions()
    {
        CreateAndParseCSVFile($"Test,              {TestEnum.TestValue},      1");

        AssertParsedValues("Test", TestEnum.TestValue, 1);
    }

    [TestMethod]
    public void CreateCSVFileWithValidValuesInIncorrectOrder_Parse_GetInvalidTypeException()
    {
        CreateAndParseCSVFile($"Test, 1, {TestEnum.TestValue}");

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    [TestMethod]
    public void CreateCSVFileWithTooFewValues_Parse_GetIncrrectNumberOfValuesException()
    {
        CreateAndParseCSVFile($"Test");

        AssertReader(typeof(IncorrectNumberOfCSVValuesException));
    }

    [TestMethod]
    public void CreateCSVFileWithTooManyValues_Parse_GetIncrrectNumberOfValuesException()
    {
        CreateAndParseCSVFile($"1, 2, 3, 4");

        AssertReader(typeof(IncorrectNumberOfCSVValuesException));
    }

    private void AssertParsedValues(string genericValue, TestEnum enumValue, int comparableValue)
    {
        AssertReader(null);
        Assert.AreEqual(genericValue, ParsedCSVData.Lines.ElementAt(0).GenericValue);
        Assert.AreEqual(enumValue, ParsedCSVData.Lines.ElementAt(0).EnumValue);
        Assert.AreEqual(comparableValue, ParsedCSVData.Lines.ElementAt(0).CompareableValue);
    }
}