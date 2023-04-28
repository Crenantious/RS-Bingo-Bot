// <copyright file="RemoveTaskRestrictionsCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;

[TestClass]
public class RemoveTaskRestrictionsCSVOperatorTests : MockDBBaseTestClass
{
    private IDataWorker dataWorkerBefore = null!;
    private IDataWorker dataWorkerAfter = null!;
    private RemoveTaskRestrictionsCSVOperator csvOperator = null!;
    private OperatorResults operatorResults = null!;
    private ReaderResults<RemoveTaskRestrictionCSVLine> readerResults = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorkerBefore = CreateDW();
        dataWorkerAfter = CreateDW();
        csvOperator = new(dataWorkerBefore);
    }

    [TestMethod]
    public void AddRestrictionToDBAndFile_ParseAndOperate_RemovedFromDBCorrectlyWithNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction = new("Restriction 1");
        CreateRestrictionsInDB(restriction);
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertRestrictions();
    }

    [TestMethod]
    public void AddARestrictionToDBAndOneToFileWithADifferentName_ParseAndOperate_NotRemovedFromDBWithAWarningAndNoExceptions()
    {
        RestrictionInfo restriction1 = new("Restriction 1");
        RestrictionInfo restriction2 = new("Restriction 2");
        CreateRestrictionsInDB(restriction1);
        CreateAndParseRestrictionsInCSVFile(restriction2);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(TaskRestrictionDoesNotExistWarning));
        AssertRestrictions(restriction1);
    }

    [TestMethod]
    public void AddRestrictionToFile_ParseAndOperate_GetAWarningAndNoExceptions()
    {
        RestrictionInfo restriction = new("Restriction 1");
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(TaskRestrictionDoesNotExistWarning));
        AssertRestrictions();
    }

    #region Private

    private void CreateAndParseRestrictionsInCSVFile(params RestrictionInfo[] restrictions) =>
        readerResults = CSVReaderTestHelper.CreateAndParseCSVFile<RemoveTaskRestrictionCSVLine>(
            restrictions.Select(r => r.Name).ToArray());

    private void CreateRestrictionsInDB(params RestrictionInfo[] restrictions) =>
        TaskRestrictionsCSVOperatorTestHelper.CreateRestrictionsInDB(dataWorkerBefore, restrictions);

    private void Operate() =>
        operatorResults = CSVOperatorTestHelper.Operate(csvOperator, readerResults.data);

    private void AssertReader(Type? exceptionType) =>
        Assert.AreEqual(exceptionType, readerResults.exceptionType);

    private void AssertOperator(Type? exceptionType, params Type[] warningTypes) =>
         CSVOperatorTestHelper.AssertOperator(new(exceptionType, warningTypes.ToList()), operatorResults);

    private void AssertRestrictions(params RestrictionInfo[] restrictions) =>
        TaskRestrictionsCSVOperatorTestHelper.AssertRestrictions(dataWorkerAfter, restrictions);

    #endregion
}