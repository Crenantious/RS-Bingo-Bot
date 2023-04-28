// <copyright file="CSVLineGenericTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.DTO;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.CSVReaderTestHelper;

[TestClass]
public class CSVLineGenericTests : MockDBBaseTestClass
{
    private ReaderResults<CSVTestLineGeneric> readerResults = null!;

    [TestMethod]
    public void AddCorrectlyTypedValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile("1");

        AssertCSVValue(1);
    }

    [TestMethod]
    public void AddIncorrectlyTypedValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile("Invalid value");

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    private void CreateAndParseCSVFile(params string[] lines) =>
        readerResults = CreateAndParseCSVFile<CSVTestLineGeneric>(lines);

    private void AssertReader(Type? exceptionType) =>
        Assert.AreEqual(exceptionType, readerResults.exceptionType);

    private void AssertCSVValue(int value) =>
        Assert.AreEqual(value, readerResults.data.Lines.ElementAt(0).Value.Value);
}