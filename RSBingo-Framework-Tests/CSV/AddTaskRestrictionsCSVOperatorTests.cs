// <copyright file="AddTaskRestrictionsCSVOperatorTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework.Models;

[TestClass]
public class AddTaskRestrictionsCSVOperatorTests : CSVOperatorTestsBase<AddTaskRestrictionsCSVOperator, AddTaskRestrictionCSVLine>
{
    [TestMethod]
    public void AddRestrictionToFile_ParseAndOperate_IsAddedToDBCorrectlyWithNoWarningMessages()
    {
        CreateRestrictionsInFile(("Restriction 1", "Description 1"));

        ParseFileAndOperate();

        AssertReaderAndOperator(null, null, 0);
        AssertRestrictions(("Restriction 1", "Description 1"));
    }

    [TestMethod]
    public void AddTwoRestrictionsWithDifferentNamesAndDescriptionsToFile_ParseAndOperate_BothAreAddedToDBWithNoWarningMessages()
    {
        CreateRestrictionsInFile(("Restriction 1", "Description 1"), ("Restriction 2", "Description 2"));

        ParseFileAndOperate();

        AssertReaderAndOperator(null, null, 0);
        AssertRestrictions(("Restriction 1", "Description 1"), ("Restriction 2", "Description 2"));
    }

    [TestMethod]
    public void AddTwoRestrictionsWithDifferentNamesAndTheSameDescriptionToFile_ParseAndOperate_BothAreAddedToDBWithNoWarningMessages()
    {
        CreateRestrictionsInFile(("Restriction 1", "Description 1"), ("Restriction 2", "Description 1"));

        ParseFileAndOperate();

        AssertReaderAndOperator(null, null, 0);
        AssertRestrictions(("Restriction 1", "Description 1"), ("Restriction 2", "Description 1"));
    }

    [TestMethod]
    public void AddTwoRestrictionsWithTheSameNameAndDifferentDescriptionsToFile_ParseAndOperate_OnlyTheFirstIsAddedToDBWithAWarningMessage()
    {
        CreateRestrictionsInFile(("Restriction 1", "Description 1"), ("Restriction 1", "Description 2"));

        ParseFileAndOperate();

        AssertReaderAndOperator(null, null, 1);
        AssertRestrictions(("Restriction 1", "Description 1"));
    }

    [TestMethod]
    public void AddTwoRestrictionsWithTheSameNameAndTheSameDescriptionToFile_ParseAndOperate_OnlyTheFirstIsAddedToDBWithAWarningMessage()
    {
        CreateRestrictionsInFile(("Restriction 1", "Description 1"), ("Restriction 1", "Description 1"));

        ParseFileAndOperate();

        AssertReaderAndOperator(null, null, 1);
        AssertRestrictions(("Restriction 1", "Description 1"));
    }

    [TestMethod]
    public void AddRestrictionToDBAndARestrictionToFileWithTheSameName_ParseAndOperate_TheRestrictionInTheFileIsNotAddedToDBWithAWarningMessage()
    {
        DataWorkerBefore.Restrictions.Create("Restriction 1", "Description 1");
        DataWorkerBefore.SaveChanges();
        CreateRestrictionsInFile(("Restriction 1", "Description 1"));

        ParseFileAndOperate();

        AssertReaderAndOperator(null, null, 1);
        AssertRestrictions(("Restriction 1", "Description 1"));
    }

    [TestMethod]
    public void AddRestrictionToDBAndARestrictionToFileWithDifferentNames_ParseAndOperate_TheRestrictionInTheFileIsAddedToDBWithNoWarningMessages()
    {
        DataWorkerBefore.Restrictions.Create("Restriction 1", "Description 1");
        DataWorkerBefore.SaveChanges();
        CreateRestrictionsInFile(("Restriction 2", "Description 1"));

        ParseFileAndOperate();

        AssertReaderAndOperator(null, null, 0);
        AssertRestrictions(("Restriction 1", "Description 1"), ("Restriction 2", "Description 1"));
    }

    private void AssertRestrictions(params (string, string)[] expectedRestrictionsInDB)
    {
        foreach((string, string) restrictionComponents in expectedRestrictionsInDB)
        {
            Restriction? restriction = DataWorkerAfter.Restrictions.GetByName(restrictionComponents.Item1);
            Assert.IsNotNull(restriction);
            Assert.AreEqual(restrictionComponents.Item2, restriction.Description);
        }

        Assert.AreEqual(expectedRestrictionsInDB.Count(), DataWorkerAfter.Restrictions.GetAll().Count());
    }

    private void CreateRestrictionsInFile(params (string, string)[] restrictions) =>
        CreateAndParseCSVFile(restrictions.Select(r => $"{r.Item1}, {r.Item2}").ToArray());
}