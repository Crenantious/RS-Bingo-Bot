// <copyright file="CSVOperatorTestsBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;

public abstract class CSVOperatorTestsBase<CSVOperatorType, CSVLineType> : CSVTestsBase<CSVLineType>
    where CSVOperatorType : CSVOperator<CSVLineType>, new()
    where CSVLineType : CSVLine
{
    private CSVOperatorType csvOperator = null!;
    private Type? operatorExceptionType = null;
    private IEnumerable<Type> warningTypes = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        csvOperator = (CSVOperatorType)Activator.CreateInstance(typeof(CSVOperatorType))!;
    }

    /// <summary>
    /// Calls <see cref="CSVOperator{LineType}.Operate(CSVData{LineType})"/> and records its exceptions and warning.
    /// </summary>
    protected void Operate()
    {
        try
        {
            csvOperator.Operate(ParsedCSVData);
            warningTypes = csvOperator.GetRawWarnings().Select(w => w.GetType());
        }
        catch (CSVOperatorException e) { operatorExceptionType = e.GetType(); }
    }

    /// <summary>
    /// Assert that the reader and operator threw exceptions of type <paramref name="readerErrorType"/>
    /// and <paramref name="operatorErrorType"/> respectively.<br/>
    /// Also assert that all <paramref name="expectedWarnings"/> were returned by the operator and in the given order.
    /// </summary>
    protected void AssertReaderAndOperator(Type? readerErrorType, Type? operatorErrorType, params Type[] expectedWarnings)
    {
        AssertReader(readerErrorType);
        AssertOperator(operatorErrorType);
        AssertOperatorWarnings(expectedWarnings);
    }

    /// <summary>
    /// Assert that the operator threw an exception of type <paramref name="exceptionType"/>.
    /// </summary>
    protected void AssertOperator(Type? exceptionType) =>
        Assert.AreEqual(operatorExceptionType, exceptionType);

    /// <summary>
    /// Assert that the operator returned warnings of types <paramref name="expectedWarningTypes"/> in the given order.
    /// </summary>
    protected void AssertOperatorWarnings(Type[] expectedWarningTypes)
    {
        Assert.AreEqual(expectedWarningTypes.Count(), warningTypes.Count());

        for (int i = 0; i < expectedWarningTypes.Length; i++)
        {
            Assert.AreEqual(expectedWarningTypes[i], warningTypes.ElementAt(i));
        }
    }
}