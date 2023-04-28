// <copyright file="CSVLineEnumCaseSensitiveTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.DTO;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;
using static RSBingo_Framework_Tests.CSV.Lines.CSVTestLineEnumBase;

[TestClass]
public class CSVLineEnumCaseSensitiveTests : MockDBBaseTestClass
{
    private ReaderResults<CSVTestLineEnumCaseSensitive> readerResults = null!;

    [TestMethod]
    public void AddEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString());

        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddLowercasedEnumValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToLower());

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    [TestMethod]
    public void AddUppercasedEnumValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToUpper());

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    [TestMethod]
    public void AddCaseInvertedEnumValueToCSVFile_Parse_GetException()
    {
        string enumValue = TestEnum.TestValue.ToString();
        string caseInvertedValue = new string(enumValue.Select(c =>
            char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c))
            .ToArray());
        CreateAndParseCSVFile(caseInvertedValue);

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    [TestMethod]
    public void AddNonEnumValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile("Invalid value");

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    private void CreateAndParseCSVFile(params string[] lines) =>
        readerResults = CreateAndParseCSVFile<CSVTestLineEnumCaseSensitive>(lines);

    private void AssertReader(Type? exceptionType) =>
        Assert.AreEqual(exceptionType, readerResults.exceptionType);

    private void AssertCSVValue(TestEnum value) =>
        Assert.AreEqual(value, readerResults.data.Lines.ElementAt(0).Value.Value);
}