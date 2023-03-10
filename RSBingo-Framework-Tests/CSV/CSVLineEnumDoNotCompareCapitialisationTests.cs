// <copyright file="CSVLineEnumDoNotCompareCapitialisationTests.cs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.CSVLines;
using static RSBingo_Framework_Tests.CSV.CSVLines.CSVTestLineEnumBase;

[TestClass]
public class CSVLineEnumDoNotCompareCapitialisationTests : CSVTestsBase<CSVTestLineEnumDoNotCompareCapitalisation>
{
    [TestMethod]
    public void AddEnumValueToCSVFile_Parse_GetCorrectValuesAndNoExceptions()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString());

        AssertReaderException(null);
        Assert.AreEqual(TestEnum.TestValue, ParsedCSVData.Lines.ElementAt(0).EnumValue);
    }

    [TestMethod]
    public void AddLowercasedEnumValueToCSVFile_Parse_GetCorrectValuesAndNoExceptions()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToLower());

        AssertReaderException(null);
        Assert.AreEqual(TestEnum.TestValue, ParsedCSVData.Lines.ElementAt(0).EnumValue);
    }

    [TestMethod]
    public void AddNonEnumValueToCSVFile_Parse_GetInvalidValueTypeException()
    {
        CreateAndParseCSVFile(Guid.NewGuid().ToString());

        AssertReaderException(typeof(InvalidCSVValueTypeException));
    }
}