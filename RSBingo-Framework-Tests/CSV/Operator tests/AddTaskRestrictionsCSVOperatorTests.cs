// <copyright file="AddTaskRestrictionsCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.CSV.Operators.Warnings;

[TestClass]
public class AddTaskRestrictionsCSVOperatorTests
{
    [TestMethod]
    public void AddRestrictionToFile_ParseAndOperate_AddedToDBCorrectlyAndThereAreNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction = new("Restriction 1", "Description 1");
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertRestrictions(restriction);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithDifferentNamesAndDescriptionsToFile_ParseAndOperate_BothAreAddedToDBCorrectlyAndThereAreNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 2", "Description 2");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertRestrictions(restriction1, restriction2);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithDifferentNamesAndTheSameDescriptionToFile_ParseAndOperate_BothAreAddedToDBCorrectlyAndThereAreNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 2", "Description 1");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertRestrictions(restriction1, restriction2);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithTheSameNameAndDifferentDescriptionsToFile_ParseAndOperate_OnlyTheFirstIsAddedToDBAndThereIsAWarningAndNoExceptions()
    {

        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 1", "Description 2");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertReaderAndOperator(null, null, typeof(TaskRestrictionAlreadyExistsWarning));
        AssertRestrictions(restriction1);
    }

    [TestMethod]
    public void AddTwoRestrictionsWithTheSameNameAndTheSameDescriptionToFile_ParseAndOperate_OnlyTheFirstIsAddedToDBAndThereIsAWarningAndNoExceptions()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 1", "Description 1");
        CreateAndParseRestrictionsInCSVFile(restriction1, restriction2);

        Operate();

        AssertReaderAndOperator(null, null, typeof(TaskRestrictionAlreadyExistsWarning));
        AssertRestrictions(restriction1);
    }

    [TestMethod]
    public void AddARestrictionToDBAndARestrictionToFileWithTheSameName_ParseAndOperate_TheRestrictionInTheFileIsNotAddedToDBAndThereIsAWarningAndNoExceptions()
    {
        RestrictionInfo restriction = new("Restriction 1", "Description 1");
        DataWorkerBefore.Restrictions.Create(restriction.Name, restriction.Description!);
        DataWorkerBefore.SaveChanges();
        CreateAndParseRestrictionsInCSVFile(restriction);

        Operate();

        AssertReaderAndOperator(null, null, typeof(TaskRestrictionAlreadyExistsWarning));
        AssertRestrictions(restriction);
    }

    [TestMethod]
    public void AddARestrictionToDBAndARestrictionToFileWithDifferentNames_ParseAndOperate_TheRestrictionInTheFileIsAddedToDBAndThereAreNoExceptionsOrWarnings()
    {
        RestrictionInfo restriction1 = new("Restriction 1", "Description 1");
        RestrictionInfo restriction2 = new("Restriction 2", "Description 1");
        DataWorkerBefore.Restrictions.Create(restriction1.Name, restriction1.Description!);
        DataWorkerBefore.SaveChanges();
        CreateAndParseRestrictionsInCSVFile(restriction2);

        Operate();

        AssertReaderAndOperator(null, null);
        AssertRestrictions(restriction1, restriction2);
    }
}