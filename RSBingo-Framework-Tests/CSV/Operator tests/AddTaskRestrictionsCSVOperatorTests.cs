// <copyright file="AddTaskRestrictionsCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;

[TestClass]
public class AddTaskRestrictionsCSVOperatorTests : MockDBBaseTestClass
{
    private IDataWorker dataWorkerBefore = null!;
    private IDataWorker dataWorkerAfter = null!;
    private AddTaskRestrictionsCSVOperator csvOperator = null!;
    private OperatorResults operatorResults = null!;
    private ReaderResults<AddTaskRestrictionCSVLine> readerResults = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        dataWorkerBefore = CreateDW();
        dataWorkerAfter = CreateDW();
        csvOperator = new(dataWorkerBefore);
    }

    [TestMethod]
    public void AddRestrictionToFile_ParseAndOperate_AddedToDBCorrectlyWithAreNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction = new("Restriction 1", "Description 1");
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertRestrictions(restriction);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithDifferentNamesAndDescriptionsToFile_ParseAndOperate_BothAreAddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 2", "Description 2");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertRestrictions(restriction1, restriction2);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithDifferentNamesAndTheSameDescriptionToFile_ParseAndOperate_BothAreAddedToDBCorrectlyWithNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 2", "Description 1");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertRestrictions(restriction1, restriction2);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithTheSameNameAndDifferentDescriptionsToFile_ParseAndOperate_OnlyTheFirstIsAddedToDBWithAWarningAndNoExceptions()
    {

        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 1", "Description 2");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(TaskRestrictionAlreadyExistsWarning));
        AssertRestrictions(restriction1);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithTheSameNameAndTheSameDescriptionToFile_ParseAndOperate_OnlyTheFirstIsAddedToDBWithAWarningAndNoExceptions()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 1", "Description 1");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(TaskRestrictionAlreadyExistsWarning));
        AssertRestrictions(restriction1);
    }

    [TestMethod]
    public void AddARestrictionToDBAndARestrictionToFileWithTheSameName_ParseAndOperate_TheRestrictionInTheFileIsNotAddedToDBWithAWarningAndNoExceptions()
    {
        RestrictionInfo restriction = new("Restriction 1", "Description 1");
        CreateRestrictionsInDB(restriction);
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertReader(null);
        AssertOperator(null, typeof(TaskRestrictionAlreadyExistsWarning));
        AssertRestrictions(restriction);
    }

    [TestMethod]
    public void AddARestrictionToDBAndARestrictionToFileWithDifferentNames_ParseAndOperate_TheRestrictionInTheFileIsAddedToDBWithNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 2", "Description 1");
        CreateRestrictionsInDB(restriction1);
        CreateAndParseRestrictionsInCSVFile(restriction2);

        Operate();

        AssertReader(null);
        AssertOperator(null);
        AssertRestrictions(restriction1, restriction2);
    }

    #region Private

    private void CreateAndParseRestrictionsInCSVFile(params RestrictionInfo[] restrictions) =>
        readerResults = CSVReaderTestHelper.CreateAndParseCSVFile<AddTaskRestrictionCSVLine>(
            restrictions.Select(r => $"{r.Name}, {r.Description}").ToArray());

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