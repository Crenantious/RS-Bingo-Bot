// <copyright file="CSVLineEnumCaseInsensitiveTests.cs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.DTO;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;
using static RSBingo_Framework_Tests.CSV.Lines.CSVTestLineEnumBase;

[TestClass]
public class CSVLineEnumCaseInsensitiveTests : MockDBBaseTestClass
{
    private ReaderResults<CSVTestLineEnumCaseInsensitive> readerResults = null!;

    [TestMethod]
    public void AddEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString());

        AssertReader(null);
        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddLowercasedEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToLower());

        AssertReader(null);
        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddUppercasedEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToUpper());

        AssertReader(null);
        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddNonEnumValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile("Invalid value".ToString());

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    private void CreateAndParseCSVFile(params string[] lines) =>
        readerResults = CreateAndParseCSVFile<CSVTestLineEnumCaseInsensitive>(lines);

    private void AssertReader(Type? exceptionType) =>
        Assert.AreEqual(exceptionType, readerResults.exceptionType);

    private void AssertCSVValue(TestEnum value) =>
        Assert.AreEqual(value, readerResults.data.Lines.ElementAt(0).Value.Value);
}