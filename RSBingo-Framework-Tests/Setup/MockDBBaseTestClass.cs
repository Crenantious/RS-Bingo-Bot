// <copyright file="MockDBBaseTestClass.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using RSBingo_Common;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;

public abstract class MockDBBaseTestClass : ServicesIntializer
{
    public TestContext TestContext { get; set; } = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        Paths.Initialise(true);
        MockDBSetup.SetupDataFactory();
        MockDBSetup.TestInitializeDB(TestContext);
    }

    [TestCleanup]
    public virtual void TestCleanup()
    {
        MockDBSetup.TestCleanUpDB(TestContext);
    }

    public IDataWorker CreateDW() => DataFactory.CreateDataWorker(TestContext.FullTestName());
}
