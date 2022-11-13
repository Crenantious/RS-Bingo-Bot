using DSharpPlus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RSBingo_Framework.Records.BingoTaskRecord;

namespace RSBingo_Framework_Tests
{
    [TestClass]
    public class DemoTests : MockDBBaseTestClass
    {
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
