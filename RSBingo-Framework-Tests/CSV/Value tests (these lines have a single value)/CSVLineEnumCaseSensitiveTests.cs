// <copyright file="CSVLineEnumCaseSensitiveTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;
using static RSBingo_Framework_Tests.CSV.Lines.CSVTestLineEnumBase;

[TestClass]
public class CSVLineEnumCaseSensitiveTests : MockDBBaseTestClass
{
    private CSVData<CSVTestLineEnumCaseSensitive> csvData;

    [TestCleanup]
    public override void TestCleanup() =>
        CSVReaderTestHelper.TestCleanup();

    [TestMethod]
    public void AddEnumValueToCSVFile_Parse_CorrectValueParsed()
    {
        CreateCSVFile(TestEnum.TestValue.ToString());

        ParseCSVFile();

        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddLowercasedEnumValueToCSVFile_Parse_GetException()
    {
        CreateCSVFile(TestEnum.TestValue.ToString().ToLower());

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    [TestMethod]
    public void AddUppercasedEnumValueToCSVFile_Parse_GetException()
    {
        CreateCSVFile(TestEnum.TestValue.ToString().ToUpper());

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    [TestMethod]
    public void AddCaseInvertedEnumValueToCSVFile_Parse_GetException()
    {
        string caseInvertedValue = InvertCase(TestEnum.TestValue.ToString());
        CreateCSVFile(caseInvertedValue);

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    [TestMethod]
    public void AddNonEnumValueToCSVFile_Parse_GetException()
    {
        CreateCSVFile("Invalid value");

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    private void ParseCSVFile() =>
        csvData = ParseCSVFile<CSVTestLineEnumCaseSensitive>();

    private static string InvertCase(string value) =>
        new string(value.Select(c =>
            char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c))
        .ToArray());

    private void AssertCSVValue(TestEnum value) =>
        Assert.AreEqual(value, csvData.Lines.ElementAt(0).Value.Value);
}