// <copyright file="TaskRestrictionsCSVOperatorTestsBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Models;

[TestClass]
public class TaskRestrictionsCSVOperatorTestsBase<CSVOperatorType, CSVLineType> : CSVOperatorTestsBase<CSVOperatorType, CSVLineType>
    where CSVOperatorType : CSVOperator<CSVLineType>, new()
    where CSVLineType : CSVLine
{
    protected record RestrictionInfo(string Name, string? Description = null);

    protected void CreateResrictionsInDB(params RestrictionInfo[] restrictions)
    {
        foreach (RestrictionInfo restriction in restrictions)
        {
            DataWorkerBefore.Restrictions.Create(restriction.Name, restriction.Description ?? "Description 1");
        }
        DataWorkerBefore.SaveChanges();
    }

    protected void CreateAndParseRestrictionsInCSVFile(params RestrictionInfo[] restrictions) =>
        CreateAndParseCSVFile(restrictions.Select(r =>
            r.Name +
            (r.Description is null ? "" : $", {r.Description}"))
            .ToArray());

    protected void AssertRestrictions(params RestrictionInfo[] expectedRestrictionsInDB)
    {
        foreach (RestrictionInfo restrictionInfo in expectedRestrictionsInDB)
        {
            Restriction? restriction = DataWorkerAfter.Restrictions.GetByName(restrictionInfo.Name);
            Assert.IsNotNull(restriction);

            if (restrictionInfo.Description is not null)
            {
                Assert.AreEqual(restrictionInfo.Description, restriction.Description);
            }
        }

        Assert.AreEqual(expectedRestrictionsInDB.Count(), DataWorkerAfter.Restrictions.CountAll());
    }
}