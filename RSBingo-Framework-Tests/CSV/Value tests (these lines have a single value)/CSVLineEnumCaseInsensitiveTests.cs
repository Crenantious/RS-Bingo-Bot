// <copyright file="CSVLineEnumCaseInsensitiveTests.cs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;
using static RSBingo_Framework_Tests.CSV.Lines.CSVTestLineEnumBase;

[TestClass]
public class CSVLineEnumCaseInsensitiveTests : MockDBBaseTestClass
{
    private CSVData<CSVTestLineEnumCaseInsensitive> csvData;

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
    public void AddLowercasedEnumValueToCSVFile_Parse_CorrectValueParsed()
    {
        CreateCSVFile(TestEnum.TestValue.ToString().ToLower());

        ParseCSVFile();

        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddUppercasedEnumValueToCSVFile_Parse_CorrectValueParsed()
    {
        CreateCSVFile(TestEnum.TestValue.ToString().ToUpper());

        ParseCSVFile();

        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddNonEnumValueToCSVFile_Parse_GetException()
    {
        CreateCSVFile("Invalid value".ToString());

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    private void ParseCSVFile() =>
        csvData = ParseCSVFile<CSVTestLineEnumCaseInsensitive>();

    private void AssertCSVValue(TestEnum value) =>
        Assert.AreEqual(value, csvData.Lines.ElementAt(0).Value.Value);
}