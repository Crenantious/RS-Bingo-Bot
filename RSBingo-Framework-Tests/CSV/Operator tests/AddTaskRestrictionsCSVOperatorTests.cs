// <copyright file="AddTaskRestrictionsCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;
using RSBingoBot.CSV;
using RSBingoBot.CSV.Lines;
using RSBingoBot.CSV.Operators.Warnings;

[TestClass]
public class AddTaskRestrictionsCSVOperatorTests : MockDBBaseTestClass
{
    private IDataWorker dataWorkerBefore = null!;
    private IDataWorker dataWorkerAfter = null!;
    private AddTaskRestrictionsCSVOperator csvOperator = null!;
    private CSVData<AddTaskRestrictionCSVLine> parsedCSVData = null!;

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
    public void AddRestrictionToFile_ParseAndOperate_AddedToDBCorrectlyWithNoWarnings()
    {
        RestrictionInfo restriction = new("Restriction 1", "Description 1");
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertOperatorWarnings();
        AssertRestrictions(restriction);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithDifferentNamesAndDescriptionsToFile_ParseAndOperate_BothAreAddedToDBCorrectlyWithNoWarnings()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 2", "Description 2");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertOperatorWarnings();
        AssertRestrictions(restriction1, restriction2);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithDifferentNamesAndTheSameDescriptionToFile_ParseAndOperate_BothAreAddedToDBCorrectlyWithNoWarnings()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 2", "Description 1");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertOperatorWarnings();
        AssertRestrictions(restriction1, restriction2);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithTheSameNameAndDifferentDescriptionsToFile_ParseAndOperate_OnlyTheFirstIsAddedToDBWithAWarning()
    {

        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 1", "Description 2");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertOperatorWarnings(typeof(TaskRestrictionAlreadyExistsWarning));
        AssertRestrictions(restriction1);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithTheSameNameAndTheSameDescriptionToFile_ParseAndOperate_OnlyTheFirstIsAddedToDBWithAWarning()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 1", "Description 1");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertOperatorWarnings(typeof(TaskRestrictionAlreadyExistsWarning));
        AssertRestrictions(restriction1);
    }

    [TestMethod]
    public void AddARestrictionToDBAndARestrictionToFileWithTheSameName_ParseAndOperate_TheRestrictionInTheFileIsNotAddedToDBWithAWarning()
    {
        RestrictionInfo restriction = new("Restriction 1", "Description 1");
        CreateRestrictionsInDB(restriction);
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertOperatorWarnings(typeof(TaskRestrictionAlreadyExistsWarning));
        AssertRestrictions(restriction);
    }

    [TestMethod]
    public void AddARestrictionToDBAndARestrictionToFileWithDifferentNames_ParseAndOperate_TheRestrictionInTheFileIsAddedToDBWithNoWarnings()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 2", "Description 1");
        CreateRestrictionsInDB(restriction1);
        CreateAndParseRestrictionsInCSVFile(restriction2);

        Operate();

        AssertOperatorWarnings();
        AssertRestrictions(restriction1, restriction2);
    }

    #region Private

    private void CreateAndParseRestrictionsInCSVFile(params RestrictionInfo[] restrictions) =>
        parsedCSVData = CSVReaderTestHelper.CreateAndParseCSVFile<AddTaskRestrictionCSVLine>(
            restrictions.Select(r => $"{r.Name}, {r.Description}").ToArray());

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