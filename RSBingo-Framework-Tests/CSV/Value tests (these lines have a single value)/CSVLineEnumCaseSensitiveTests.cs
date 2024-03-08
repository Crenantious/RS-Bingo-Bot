// <copyright file="CSVLineEnumCaseSensitiveTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework_Tests.CSV.Lines;
using RSBingoBot.CSV;
using RSBingoBot.CSV.Exceptions;
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
    [DataRow("testvalue")]
    [DataRow("TESTVALUE")]
    [DataRow("Invalid value")]
    public void AddInvalidValuesWithMixedCasesToCSVFile_Parse_GetException(string testValue)
    {
        CreateCSVFile(testValue);

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    private void ParseCSVFile() =>
        csvData = ParseCSVFile<CSVTestLineEnumCaseSensitive>();

    private void AssertCSVValue(TestEnum value) =>
        Assert.AreEqual(value, csvData.Lines.ElementAt(0).Value.Value);
}