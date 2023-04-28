// <copyright file="CSVLineCompareableTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.DTO;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;
using static RSBingo_Framework_Tests.CSV.Lines.CSVTestLineComparable;

[TestClass]
public class CSVLineCompareableTests : MockDBBaseTestClass
{
    private ReaderResults<CSVTestLineComparable> readerResults = null!;

    [TestMethod]
    public void AddMinValueToCSVFile_Parse_NoExceptions()
    {
        CreateAndParseCSVFile(ComparableValueMin.ToString());

        AssertReader(null);
        AssertCSVValue(ComparableValueMin);
    }

    [TestMethod]
    public void AddMaxValueToCSVFile_Parse_NoExceptions()
    {
        CreateAndParseCSVFile(ComparableValueMax.ToString());

        AssertReader(null);
        AssertCSVValue(ComparableValueMax);
    }

    [TestMethod]
    public void AddValueInbetweenMinAndMaxToCSVFile_Parse_NoExceptions()
    {
        CreateAndParseCSVFile((ComparableValueMax - 1).ToString());

        AssertReader(null);
        AssertCSVValue(ComparableValueMax - 1);
    }

    [TestMethod]
    public void AddValueToCSVFileThatIsTooLargeToConvert_Parse_GetException()
    {
        CreateAndParseCSVFile(Int32.MaxValue.ToString() + "0");

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    [TestMethod]
    public void AddValueToCSVFileThatIsTooSmallToConvert_Parse_GetException()
    {
        CreateAndParseCSVFile(Int32.MinValue.ToString() + "0");

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    [TestMethod]
    public void AddValueLessThanMinToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile((ComparableValueMin - 1).ToString());

        AssertReader(typeof(CSVValueOutOfRangeException));
    }

    [TestMethod]
    public void AddValueGreaterThanMaxToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile((ComparableValueMax + 1).ToString());

        AssertReader(typeof(CSVValueOutOfRangeException));
    }

    [TestMethod]
    public void AddIncorrectlyTypedValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile("1.23");

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    private void CreateAndParseCSVFile(params string[] lines) =>
       readerResults = CreateAndParseCSVFile<CSVTestLineComparable>(lines);

    private void AssertReader(Type? exceptionType) =>
        Assert.AreEqual(exceptionType, readerResults.exceptionType);

    private void AssertCSVValue(int value) =>
        Assert.AreEqual(value, readerResults.data.Lines.ElementAt(0).Value.Value);
}