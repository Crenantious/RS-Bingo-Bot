// <copyright file="CSVOperatorTestsBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.DTO;

public static class CSVOperatorTestHelper
{
    /// <summary>
    /// Calls <see cref="CSVOperator{LineType}.Operate(CSVData{LineType})"/> and records its exceptions and warning.
    /// </summary>
    /// <returns>A <see cref="OperatorResults"/> containing the warning is successful, or exception type otherwise.</returns>
    public static OperatorResults Operate<LineType>(CSVOperator<LineType> csvOperator, CSVData<LineType> parsedCSVData)
        where LineType : CSVLine
    {
        OperatorResults operatorResults;
        try
        {
            csvOperator.Operate(parsedCSVData);
            operatorResults =  new(null, csvOperator.GetRawWarnings().Select(w => w.GetType()).ToList());
        }
        catch (CSVOperatorException e)
        { 
            operatorResults = new(e.GetType(), Enumerable.Empty<Type>().ToList());
        }

        return operatorResults;
    }

    /// <summary>
    /// Assert that the reader and operator threw exceptions of type <paramref name="readerErrorType"/>
    /// and <paramref name="operatorErrorType"/> respectively.<br/>
    /// Also assert that all <paramref name="expectedWarnings"/> were returned by the operator and in the given order.
    /// </summary>
    /// <param name="actualResults">The actual results of the test.</param>
    /// <param name="expectedResults">The expected results of the test.</param>
    public static void AssertOperator(OperatorResults expectedResults, OperatorResults actualResults)
    {
        Assert.AreEqual(expectedResults.OperatorExceptionType, actualResults.OperatorExceptionType);
        CollectionAssert.AreEqual(expectedResults.WarningTypes, actualResults.WarningTypes);
    }
}