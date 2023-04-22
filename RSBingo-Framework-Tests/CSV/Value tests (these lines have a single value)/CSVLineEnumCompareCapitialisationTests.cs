// <copyright file="CSVLineEnumCompareCapitialisationTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.CSV.Lines;
using static RSBingo_Framework_Tests.CSV.Lines.CSVTestLineEnumBase;

[TestClass]
public class CSVLineEnumCompareCapitialisationTests : CSVTestsBase<CSVTestLineEnumCompareCapitalisation>
{
    [TestMethod]
    public void AddEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString());

        Assert.AreEqual(TestEnum.TestValue, ParsedCSVData.Lines.ElementAt(0).Value.Value);
    }

    [TestMethod]
    public void AddLowercasedEnumValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToLower());

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    [TestMethod]
    public void AddUppercasedEnumValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToUpper());

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    [TestMethod]
    public void AddNonEnumValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile(Guid.NewGuid().ToString());

        AssertReader(typeof(InvalidCSVValueTypeException));
    }
}