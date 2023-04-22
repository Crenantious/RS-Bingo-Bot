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

    private CSVReader reader = new CSVReader();
    private Type? readerExceptionType = null;

    protected IDataWorker DataWorkerBefore { get; private set; } = null!;
    protected IDataWorker DataWorkerAfter { get; private set; } = null!;

    /// <summary>
    /// Data parsed by a <see cref="CSVReader"/>.<br/>
    /// Only set once <see cref="CreateAndParseCSVFile"/> has been called and the reader had no exceptions.
    /// </summary>
    protected CSVData<CSVLineType> ParsedCSVData { get; private set; }

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

    protected void CreateAndParseCSVFile(params string[] lines)
    {
        CreateCSVFile(lines);
        ParseFile(CSVFileName);
    }

    protected void CreateAndParseFile(string fileName)
    {
        File.Create(fileName);
        ParseFile(fileName);
    }

    /// <summary>
    /// Assert that the reader threw an exception of type <paramref name="exceptionType"/>.
    /// </summary>
    protected void AssertReader(Type? exceptionType) =>
        Assert.AreEqual(readerExceptionType, exceptionType);

    private void CreateCSVFile(params string[] lines) =>
        File.WriteAllLines(CSVFileName, lines);

    private void ParseFile(string fileName)
    {
        try { ParsedCSVData = reader.Parse<CSVLineType>(fileName); }
        catch (CSVReaderException e) { readerExceptionType = e.GetType(); }
    }
}