// <copyright file="TaskRestrictionsCSVOperatorTestsBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.CSV;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework_Tests.DTO;

public class TaskRestrictionsCSVOperatorTestHelper
{
    private IDataWorker DataWorkerBefore;

    public TaskRestrictionsCSVOperatorTestHelper(IDataWorker dataWorker)
    {
        DataWorkerBefore = dataWorker;
    }

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

    protected void AssertRestrictions(IDataWorker dataWorkerAfter, params RestrictionInfo[] expectedRestrictionsInDB)
    {
        foreach (RestrictionInfo restrictionInfo in expectedRestrictionsInDB)
        {
            Restriction? restriction = dataWorkerAfter.Restrictions.GetByName(restrictionInfo.Name);
            Assert.IsNotNull(restriction);

            if (restrictionInfo.Description is not null)
            {
                Assert.AreEqual(restrictionInfo.Description, restriction.Description);
            }
        }

        Assert.AreEqual(expectedRestrictionsInDB.Count(), dataWorkerAfter.Restrictions.CountAll());
    }
}