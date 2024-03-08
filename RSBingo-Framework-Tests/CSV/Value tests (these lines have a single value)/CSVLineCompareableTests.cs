// <copyright file="CSVLineCompareableTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;
using RSBingo_Framework_Tests.CSV.Lines;
using RSBingoBot.CSV;
using RSBingoBot.CSV.Exceptions;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;
using static RSBingo_Framework_Tests.CSV.Lines.CSVTestLineComparable;

[TestClass]
public class CSVLineCompareableTests : MockDBBaseTestClass
{
    private CSVData<CSVTestLineComparable> csvData;

    [TestCleanup]
    public override void TestCleanup() =>
        CSVReaderTestHelper.TestCleanup();

    [TestMethod]
    public void AddMinValueToCSVFile_Parse_CorrectValueParsed()
    {
        CreateCSVFile(ComparableValueMin.ToString());

        ParseCSVFile();

        AssertCSVValue(ComparableValueMin);
    }

    [TestMethod]
    public void AddMaxValueToCSVFile_Parse_CorrectValueParsed()
    {
        CreateCSVFile(ComparableValueMax.ToString());

        ParseCSVFile();

        AssertCSVValue(ComparableValueMax);
    }

    [TestMethod]
    public void AddValueInbetweenMinAndMaxToCSVFile_Parse_CorrectValueParsed()
    {
        CreateCSVFile((ComparableValueMax - 1).ToString());

        ParseCSVFile();

        AssertCSVValue(ComparableValueMax - 1);
    }

    [TestMethod]
    public void AddValueToCSVFileThatIsTooLargeToConvert_Parse_GetException()
    {
        CreateCSVFile(Int32.MaxValue.ToString() + "0");

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    [TestMethod]
    public void AddValueToCSVFileThatIsTooSmallToConvert_Parse_GetException()
    {
        CreateCSVFile(Int32.MinValue.ToString() + "0");

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    [TestMethod]
    public void AddValueLessThanMinToCSVFile_Parse_GetException()
    {
        CreateCSVFile((ComparableValueMin - 1).ToString());

        Assert.ThrowsException<CSVValueOutOfRangeException>(() => ParseCSVFile());
    }

    [TestMethod]
    public void AddValueGreaterThanMaxToCSVFile_Parse_GetException()
    {
        CreateCSVFile((ComparableValueMax + 1).ToString());

        Assert.ThrowsException<CSVValueOutOfRangeException>(() => ParseCSVFile());
    }

    [TestMethod]
    public void AddIncorrectlyTypedValueToCSVFile_Parse_GetException()
    {
        CreateCSVFile("1.23");

        Assert.ThrowsException<InvalidCSVValueTypeException>(() => ParseCSVFile());
    }

    private void ParseCSVFile() =>
       csvData = ParseCSVFile<CSVTestLineComparable>();

    private void AssertCSVValue(int value) =>
        Assert.AreEqual(value, csvData.Lines.ElementAt(0).Value.Value);
}