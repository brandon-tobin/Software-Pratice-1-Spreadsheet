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
            IEnumerable values = test.GetDependents("hello");
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
            IEnumerable values = test.GetDependees("world");
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
            Assert.AreEqual(true, test.HasDependents("hello"));
        }

        [TestMethod]
        public void HasDependents2()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3500");
            test.AddDependency("cs", "3810");
            Assert.AreEqual(false, test.HasDependents("visual"));
        }

        [TestMethod]
        public void HasDependees1()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3500");
            test.AddDependency("cs", "3810");
            Assert.AreEqual(true, test.HasDependees("world"));
        }

        [TestMethod]
        public void HasDependees2()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3500");
            test.AddDependency("cs", "3810");
            Assert.AreEqual(false, test.HasDependees("visual"));
        }

        [TestMethod]
        public void RemoveDependecy1()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            test.RemoveDependency("hello", "world");
            IEnumerable values = test.GetDependents("hello");
            String temp = "";
            foreach (String s in values)
            {
                temp += s;
            }
            Assert.AreEqual("you", temp);
        }

        [TestMethod]
        public void RemoveDependecy2()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("cs", "3500");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3810");
            test.AddDependency("cs", "2420");
            test.RemoveDependency("cs", "3500");
            IEnumerable values = test.GetDependents("cs");
            String temp = "";
            foreach (String s in values)
            {
                temp += s;
            }
            Assert.AreEqual("38102420", temp);
        }

        [TestMethod]
        public void RemoveDependecy3()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("cs", "3500");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3810");
            test.AddDependency("cs", "2420");
            test.RemoveDependency("cs", "3500");
            IEnumerable values = test.GetDependents("hello");
            String temp = "";
            foreach (String s in values)
            {
                temp += s;
            }
            Assert.AreEqual("worldyou", temp);
        }
    }
}
