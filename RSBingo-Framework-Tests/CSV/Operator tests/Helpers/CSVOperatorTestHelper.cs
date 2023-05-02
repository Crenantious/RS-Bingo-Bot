// <copyright file="CSVOperatorTestHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework_Tests.DTO;

public static class CSVOperatorTestHelper
{
    /// <summary>
    /// Calls <see cref="CSVOperator{LineType}.Operate(CSVData{LineType})"/> and records its possible exception and warnings.
    /// </summary>
    /// <returns><see cref="OperatorResults"/> containing the exception, if one occurred. The warnings, otherwise.</returns>
    public static OperatorResults Operate<LineType>(CSVOperator<LineType> csvOperator, CSVData<LineType> parsedCSVData)
        where LineType : CSVLine
    {
        csvOperator.Operate(parsedCSVData);
        return new(null, csvOperator.GetRawWarnings().Select(w => w.GetType()).ToList());
    }

    /// <summary>
    /// Assert that <paramref name="expectedResults"/> and <paramref name="actualResults"/> contain the same data.
    /// </summary>
    /// <param name="expectedResults">The expected results of an operator.</param>
    /// <param name="actualResults">The actual results of an operator.</param>
    public static void AssertOperator(OperatorResults expectedResults, OperatorResults actualResults)
    {
        Assert.AreEqual(expectedResults.ExceptionType, actualResults.ExceptionType);
        CollectionAssert.AreEqual(expectedResults.WarningTypes, actualResults.WarningTypes);
    }
}