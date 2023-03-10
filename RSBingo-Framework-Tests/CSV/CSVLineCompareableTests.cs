// <copyright file="CSVLineCompareableTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.CSVLines;
using static RSBingo_Framework_Tests.CSV.CSVLines.CSVTestLineComparable;

[TestClass]
public class CSVLineCompareableTests : CSVTestsBase<CSVTestLineComparable>
{
    [TestMethod]
    public void AddMinValueToCSVFile_Parse_NoErrorsOrWarnings()
    {
        CreateAndParseCSVFile(ComparableValueMin.ToString());

        AssertReaderException(null);
        AssertCSVValue(ComparableValueMin);
    }

    [TestMethod]
    public void AddMaxValueToCSVFile_Parse_NoErrorsOrWarnings()
    {
        CreateAndParseCSVFile(ComparableValueMax.ToString());

        AssertReaderException(null);
        AssertCSVValue(ComparableValueMax);
    }

    [TestMethod]
    public void AddValueInbetweenMinAndMaxToCSVFile_Parse_NoErrorsOrWarnings()
    {
        CreateAndParseCSVFile((ComparableValueMax - 1).ToString());

        AssertReaderException(null);
        AssertCSVValue(ComparableValueMax - 1);
    }

    [TestMethod]
    public void AddValueLessThanMinToCSVFile_Parse_GetCSVValueOutOfRangeException()
    {
        CreateAndParseCSVFile((ComparableValueMin - 1).ToString());

        AssertReaderException(typeof(CSVValueOutOfRangeException));
    }

    [TestMethod]
    public void AddValueGreaterThanMaxToCSVFile_Parse_GetCSVValueOutOfRangeException()
    {
        CreateAndParseCSVFile((ComparableValueMax + 1).ToString());

        AssertReaderException(typeof(CSVValueOutOfRangeException));
    }

    [TestMethod]
    public void AddIncorrectlyTypedValueToCSVFile_Parse_GetInvalidValueTypeException()
    {
        CreateAndParseCSVFile("1.23");

        AssertReaderException(typeof(InvalidCSVValueTypeException));
    }

    private void AssertCSVValue(object value) =>
        Assert.AreEqual(value, ParsedCSVData.Lines.ElementAt(0).ComparableValue);
}