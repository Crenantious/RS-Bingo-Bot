// <copyright file="DemoTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Records.BingoTaskRecord;

    [TestClass]
    public class DemoTests : MockDBBaseTestClass
    {
        // TODO: JCH - Once we have real tests delete this class as it is only here as an example.

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestMethod]
        public void Example()
        {
            IDataWorker dataWorkerBefore = CreateDW();
            int bingoTaskCountBefore = dataWorkerBefore.BingoTasks.CountAll();

            BingoTask bingoTask = MockDBSetup.Add_BingoTask(dataWorkerBefore, "Test", difficulty: Difficulty.Easy);
            IDataWorker dataWorkerAfter = CreateDW();
            List<BingoTask> bingoTasksAfter = dataWorkerAfter.BingoTasks.GetAll().ToList();

            Assert.AreEqual(1, bingoTasksAfter.Count - bingoTaskCountBefore);
        }
    }
}
