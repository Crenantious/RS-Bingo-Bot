// <copyright file="CSVLineGenericTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.CSVLines;

[TestClass]
public class CSVLineGenericTests : CSVTestsBase<CSVTestLineGeneric>
{
    [TestMethod]
    public void AddCorrectlyTypedValueToCSVFile_Parse_NoErrorsOrWarnings()
    {
        CreateAndParseCSVFile("1");

        Assert.AreEqual(1, ParsedCSVData.Lines.ElementAt(0).GenericValue);
    }

    [TestMethod]
    public void AddIncorrectlyTypedValueToCSVFile_Parse_GetInvalidValueTypeException()
    {
        CreateAndParseCSVFile("string");

        AssertReaderException(typeof(InvalidCSVValueTypeException));
    }
}