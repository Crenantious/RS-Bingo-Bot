using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBingo_Framework_Tests
{
    public static class Extensions
    {
        public static string FullTestName(this TestContext testContext) => $"{testContext.FullyQualifiedTestClassName}.{testContext.TestName}";

    }
}
