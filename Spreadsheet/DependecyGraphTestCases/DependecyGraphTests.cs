using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections;

namespace DependecyGraphTestCases
{
    [TestClass]
    public class DependecyGraphTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("fuck", "you");
            test.AddDependency("fuck", "hi");
            IEnumerable values = test.GetDependees("fuck");
            Assert.AreEqual(test.GetDependees("fuck"), "you, hi");
        }
    }
}
