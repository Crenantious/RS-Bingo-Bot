// <copyright file="CSVTestsBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework.Interfaces;

public abstract class CSVTestsBase<CSVLineType> : MockDBBaseTestClass
    where CSVLineType : CSVLine
{
    private readonly string CSVFileName = Path.GetTempPath() + "Test.csv";

    private string currentFileName = string.Empty;

    protected IDataWorker DataWorkerBefore = null!;
    protected IDataWorker DataWorkerAfter = null!;
    protected Exception? CSVReaderException = null;
    protected CSVData<CSVLineType> ParsedCSVData;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        DataWorkerBefore = CreateDW();
        DataWorkerAfter = CreateDW();
    }

    [TestCleanup]
    public override void TestCleanup() =>
        File.Delete(CSVFileName);

    private void CreateCSVFile(params string[] lines)
    {
        File.WriteAllLines(CSVFileName, lines);
        currentFileName = CSVFileName;
    }

    protected void CreateAndParseCSVFile(params string[] lines)
    {
        CreateCSVFile(lines);
        ParseFile();
    }

    protected void CreateAndParseFile(string name)
    {
        File.Create(name);
        currentFileName = name;
        ParseFile();
    }

    protected void ParseFile()
    {
        try { ParsedCSVData = CSVReader.Parse<CSVLineType>(currentFileName); }
        catch (CSVReaderException e) { CSVReaderException = e; }
    }

    protected void AssertReader(Type? readerExceptionType)
    {
        if (readerExceptionType is null || CSVReaderException is null)
        {
            Assert.AreEqual(readerExceptionType, CSVReaderException);
            return;
        }

        Assert.AreEqual(readerExceptionType, CSVReaderException.GetType());
    }
}