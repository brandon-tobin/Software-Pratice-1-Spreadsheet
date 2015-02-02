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
        public void AddDependency1()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            IEnumerable values = test.GetDependees("hello");
            String temp = "";
            foreach (String s in values)
            {
                temp += s;
            }
            Assert.AreEqual(temp, "worldyou");
        }

        [TestMethod]
        public void AddDependency2()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            IEnumerable values = test.GetDependents("world");
            String temp = "";
            foreach (String s in values)
            {
                temp += s;
            }
            Assert.AreEqual(temp, "hello");
        }

        [TestMethod]
        public void AddDependency3()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            IEnumerable values = test.GetDependees("testing");
            String temp = "";
            foreach (String s in values)
            {
                temp += s;
            }
            Assert.AreEqual(temp, "");
        }

        [TestMethod]
        public void AddDependency4()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            IEnumerable values = test.GetDependents("testing");
            String temp = "";
            foreach (String s in values)
            {
                temp += s;
            }
            Assert.AreEqual(temp, "");
        }

        [TestMethod]
        public void AddDependency5()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3500");
            test.AddDependency("cs", "3810");
            
            Assert.AreEqual(test.Size, 4);
        }

        [TestMethod]
        public void HasDependents1()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3500");
            test.AddDependency("cs", "3810");
            Assert.AreEqual(test.HasDependents("hello"), true);
        }

        [TestMethod]
        public void HasDependents2()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3500");
            test.AddDependency("cs", "3810");
            Assert.AreEqual(test.HasDependents("visual"), false);
        }
    }
}
