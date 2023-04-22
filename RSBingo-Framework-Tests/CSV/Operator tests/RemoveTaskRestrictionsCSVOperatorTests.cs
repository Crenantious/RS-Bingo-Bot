// <copyright file="RemoveTaskRestrictionsCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;

[TestClass]
public class RemoveTaskRestrictionsCSVOperatorTests : TaskRestrictionsCSVOperatorTestsBase<RemoveTaskRestrictionsCSVOperator, RemoveTaskRestrictionCSVLine>
{
    [TestMethod]
    public void AddRestrictionToDBAndFile_ParseAndOperate_RemovedFromDBCorrectlyWithNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction = new("Restriction 1");
        CreateResrictionsInDB(restriction);
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertRestrictions();
    }

    [TestMethod]
    public void AddARestrictionToDBAndOneToFileWithADifferentName_ParseAndOperate_NotRemovedFromDBAndGetAWarningAndNoExceptions()
    {
        RestrictionInfo restriction1 = new("Restriction 1");
        RestrictionInfo restriction2 = new("Restriction 2");
        CreateResrictionsInDB(restriction1);
        CreateAndParseRestrictionsInCSVFile(restriction2);

        Operate();

        AssertReaderAndOperator(null, null, typeof(TaskRestrictionDoesNotExistWarning));
        AssertRestrictions(restriction1);
    }

    [TestMethod]
    public void AddRestrictionToFile_ParseAndOperate_GetAWarningAndNoExceptions()
    {
        RestrictionInfo restriction = new("Restriction 1");
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertReaderAndOperator(null, null, typeof(TaskRestrictionDoesNotExistWarning));
        AssertRestrictions();
    }
}