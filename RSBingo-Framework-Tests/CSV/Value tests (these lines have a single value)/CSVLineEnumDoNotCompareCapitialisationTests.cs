// <copyright file="CSVLineEnumDoNotCompareCapitialisationTests.cs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

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
        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddLowercasedEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToLower());

        AssertReader(null);
        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddUppercasedEnumValueToCSVFile_Parse_GetCorrectValue()
    {
        CreateAndParseCSVFile(TestEnum.TestValue.ToString().ToUpper());

        AssertReader(null);
        AssertCSVValue(TestEnum.TestValue);
    }

    [TestMethod]
    public void AddNonEnumValueToCSVFile_Parse_GetException()
    {
        CreateAndParseCSVFile(Guid.NewGuid().ToString());

        AssertReader(typeof(InvalidCSVValueTypeException));
    }

    private void AssertCSVValue(TestEnum value) =>
        Assert.AreEqual(value, ParsedCSVData.Lines.ElementAt(0).Value.Value);
}