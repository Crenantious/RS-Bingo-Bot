// <copyright file="TaskRestrictionsCSVOperatorTestHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV;

using RSBingo_Framework.Models;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework_Tests.DTO;

public class TaskRestrictionsCSVOperatorTestHelper
{
    public static void CreateResrictionsInDB(IDataWorker dataWorker, params RestrictionInfo[] restrictions)
    {
        foreach (RestrictionInfo restriction in restrictions)
        {
            dataWorker.Restrictions.Create(restriction.Name, restriction.Description ?? "Description 1");
        }
        dataWorker.SaveChanges();
    }

    public static void CreateRestrictionsInDB(IDataWorker dataWorker, params RestrictionInfo[] restrictions)
    {
        foreach (RestrictionInfo restriction in restrictions)
        {
            dataWorker.Restrictions.Create(restriction.Name, restriction.Description);
        }
        dataWorker.SaveChanges();
    }

    public static void AssertRestrictions(IDataWorker dataWorkerAfter, params RestrictionInfo[] expectedRestrictionsInDB)
    {
        foreach (RestrictionInfo restrictionInfo in expectedRestrictionsInDB)
        {
            Restriction? restriction = dataWorkerAfter.Restrictions.GetByName(restrictionInfo.Name);
            Assert.IsNotNull(restriction);
            Assert.AreEqual(restrictionInfo.Description, restriction.Description);
        }

        Assert.AreEqual(expectedRestrictionsInDB.Count(), dataWorkerAfter.Restrictions.CountAll());
    }
}