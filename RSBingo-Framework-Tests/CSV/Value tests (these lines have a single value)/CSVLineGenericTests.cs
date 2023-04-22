// <copyright file="CSVLineGenericTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.Lines;

[TestClass]
public class CSVLineGenericTests : CSVTestsBase<CSVTestLineGeneric>
{
    [TestMethod]
    public void AddCorrectlyTypedValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile("1");

        Assert.AreEqual(1, ParsedCSVData.Lines.ElementAt(0).Value.Value);
    }

    [TestMethod]
    public void AddIncorrectlyTypedValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile("string");

        AssertReader(typeof(InvalidCSVValueTypeException));
    }
}