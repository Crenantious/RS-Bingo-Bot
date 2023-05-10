// <copyright file="CSVLineGenericTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;

[TestClass]
public class CSVLineGenericTests : MockDBBaseTestClass
{
    private CSVData<CSVTestLineGeneric> csvData;

    [TestCleanup]
    public override void TestCleanup() =>
        CSVReaderTestHelper.TestCleanup();

    [TestMethod]
    public void AddCorrectlyTypedValueToCSVFile_Parse_CorrectValueParsed()
    { 
        CreateCSVFile("1");

        ParseCSVFile();

        AssertCSVValue(1);
    }

    [TestMethod]
    public void AddIncorrectlyTypedValueToCSVFile_Parse_GetException()
    {
        CreateCSVFile("Invalid value");

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    private void ParseCSVFile() =>
        csvData = ParseCSVFile<CSVTestLineGeneric>();

    private void AssertCSVValue(int value) =>
        Assert.AreEqual(value, csvData.Lines.ElementAt(0).Value.Value);
}