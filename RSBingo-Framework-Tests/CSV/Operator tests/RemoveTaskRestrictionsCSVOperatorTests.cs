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
    private CSVData<RemoveTaskRestrictionCSVLine> parsedCSVData = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorkerBefore = CreateDW();
        dataWorkerAfter = CreateDW();
        csvOperator = new(dataWorkerBefore);
    }

    [TestCleanup]
    public override void TestCleanup()
    {
        CSVReaderTestHelper.TestCleanup();
        BingoTasksCSVOperatorTestHelper.TestCleanup();
    }

    [TestMethod]
    public void AddRestrictionToDBAndFile_ParseAndOperate_RemovedFromDBCorrectlyWithNoWarnings()
    {
        RestrictionInfo restriction = new("Restriction 1");
        CreateRestrictionsInDB(restriction);
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertOperatorWarnings();
        AssertRestrictions();
    }

    [TestMethod]
    public void AddARestrictionToDBAndOneToFileWithADifferentName_ParseAndOperate_NotRemovedFromDBWithAWarning()
    {
        RestrictionInfo restriction1 = new("Restriction 1");
        RestrictionInfo restriction2 = new("Restriction 2");
        CreateRestrictionsInDB(restriction1);
        CreateAndParseRestrictionsInCSVFile(restriction2);

        Operate();

        AssertOperatorWarnings(typeof(TaskRestrictionDoesNotExistWarning));
        AssertRestrictions(restriction1);
    }

    [TestMethod]
    public void AddRestrictionToFile_ParseAndOperate_GetAWarning()
    {
        RestrictionInfo restriction = new("Restriction 1");
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertOperatorWarnings(typeof(TaskRestrictionDoesNotExistWarning));
        AssertRestrictions();
    }

    #region Private

    private void CreateAndParseRestrictionsInCSVFile(params RestrictionInfo[] restrictions) =>
        parsedCSVData = CSVReaderTestHelper.CreateAndParseCSVFile<RemoveTaskRestrictionCSVLine>(
            restrictions.Select(r => $"{r.Name}").ToArray());

    private void CreateRestrictionsInDB(params RestrictionInfo[] restrictions) =>
        TaskRestrictionsCSVOperatorTestHelper.CreateRestrictionsInDB(dataWorkerBefore, restrictions);

    private void Operate() =>
        csvOperator.Operate(parsedCSVData);

    private void AssertOperatorWarnings(params Type[] warningTypes) =>
        CollectionAssert.AreEqual(warningTypes, csvOperator.GetRawWarnings().Select(w => w.GetType()).ToArray());

    private void AssertRestrictions(params RestrictionInfo[] restrictions) =>
        TaskRestrictionsCSVOperatorTestHelper.AssertRestrictions(dataWorkerAfter, restrictions);

    #endregion
}