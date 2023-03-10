// <copyright file="CSVLineEnumDoNotCompareCapitialisationTests.cs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.Lines.CSVTestLineEnumBase;

[TestClass]
public class CSVLineEnumDoNotCompareCapitialisationTests : CSVTestsBase<CSVTestLineEnumDoNotCompareCapitalisation>
{
    [TestMethod]
    public void AddEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString());

        AssertReader(null);
        Assert.AreEqual(TestEnum.TestValue, ParsedCSVData.Lines.ElementAt(0).EnumValue);
    }

    [TestMethod]
    public void AddLowercasedEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToLower());

        AssertReader(null);
        Assert.AreEqual(TestEnum.TestValue, ParsedCSVData.Lines.ElementAt(0).EnumValue);
    }

    [TestMethod]
    public void AddUppercasedEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToUpper());

        AssertReader(null);
        Assert.AreEqual(TestEnum.TestValue, ParsedCSVData.Lines.ElementAt(0).EnumValue);
    }

    [TestMethod]
    public void AddNonEnumValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile(Guid.NewGuid().ToString());

        AssertReader(typeof(InvalidCSVValueTypeException));
    }
}