// <copyright file="CSVOperatorTestsBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework.Interfaces;

public abstract class CSVOperatorTestsBase<CSVOperatorType, CSVLineType> : CSVTestsBase<CSVLineType>
    where CSVOperatorType : CSVOperator<CSVLineType>, new()
    where CSVLineType : CSVLine
{
    protected CSVOperatorType CSVOperator = null!;
    protected Exception? CSVOperatorException = null;
    protected int CSVOperatorWarningCount = 0;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        DataWorkerBefore = CreateDW();
        DataWorkerAfter = CreateDW();
        CSVOperator = (CSVOperatorType)Activator.CreateInstance(typeof(CSVOperatorType));
    }

    protected void Operate()
    {
        try
        {
            CSVOperator.Operate(ParsedCSVData);
            CSVOperatorWarningCount = CSVOperator.GetRawWarnings().Count();
        }
        catch (CSVOperatorException e) { CSVOperatorException = e; }
    }

    protected void AssertReaderAndOperator(Type? readerErrorType, Type? operatorErrorType, int operatorWarningsCount)
    {
        Assert.AreEqual(readerErrorType, CSVReaderException);
        Assert.AreEqual(operatorErrorType, CSVOperatorException);
        Assert.AreEqual(operatorWarningsCount, operatorWarningsCount);
    }
}