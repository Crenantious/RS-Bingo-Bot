using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBingo_Framework_Tests
{
    public abstract class MockDBBaseTestClass
    {
        public TestContext TestContext { get; set; } = null!;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            MockDBSetup.TestInitializeDB(TestContext);
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            MockDBSetup.TestCleanUpDB(TestContext);
        }

        public IDataWorker CreateDW()  => DataFactory.CreateDataWorker(TestContext.FullTestName());
    }
}
