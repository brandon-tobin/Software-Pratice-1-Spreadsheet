using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections;
using System.Collections.Generic;

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
            Assert.AreEqual("worldyou", temp);
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
            Assert.AreEqual("hello", temp);
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
            Assert.AreEqual("", temp);
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
            Assert.AreEqual("", temp);
        }

        // Test addDependency if the dependency already exists 
        [TestMethod]
        public void AddDependency5()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            test.AddDependency("hello", "world");

            IEnumerable values = test.GetDependents("hello");
            String temp = "";
            foreach (String s in values)
            {
                temp += s;
            }
            Assert.AreEqual("worldyou", temp);
        }

        [TestMethod]
        public void Size1()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3500");
            test.AddDependency("cs", "3810");
            
            Assert.AreEqual(4, test.Size);
        }

        [TestMethod]
        public void Size2()
        {
            DependencyGraph test = new DependencyGraph();
            for (int i = 0; i < 50; i++)
            {
                String temp = i.ToString();
                test.AddDependency(temp, temp);
            }

            Assert.AreEqual(50, test.Size);
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

        // Test to see if remove does anything if the dependency is not there
        [TestMethod]
        public void RemoveDependecy4()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("cs", "3500");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3810");
            test.AddDependency("cs", "2420");
            test.RemoveDependency("cs", "4540");
            IEnumerable values = test.GetDependents("cs");
            String temp = "";
            foreach (String s in values)
            {
                temp += s;
            }
            Assert.AreEqual("350038102420", temp);
        }

        [TestMethod]
        public void Replace1()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("cs", "3500");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3810");
            test.AddDependency("cs", "2420");
           
            List<String> temp = new List<String>();
            temp.Add("tom");
            temp.Add("scott");
            temp.Add("brendan");
            temp.Add("utah");
            test.ReplaceDependents("hello", temp);

            String dependents = "";
            IEnumerable values = test.GetDependents("hello");
            foreach (String s in values)
            {
                dependents += s;
            }
            Assert.AreEqual("tomscottbrendanutah", dependents);
        }

        [TestMethod]
        public void Replace2()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("cs", "3500");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3810");
            test.AddDependency("cs", "2420");

            List<String> temp = new List<String>();
            temp.Add("tom");
            temp.Add("scott");
            temp.Add("brendan");
            temp.Add("utah");
            test.ReplaceDependents("hello", temp);

            String dependents = "";
            IEnumerable values = test.GetDependents("cs");
            foreach (String s in values)
            {
                dependents += s;
            }
            Assert.AreEqual("350038102420", dependents);
        }

        [TestMethod]
        public void Replace3()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("cs", "3500");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3810");
            test.AddDependency("cs", "2420");

            List<String> temp = new List<String>();
            temp.Add("tom");
            temp.Add("scott");
            temp.Add("brendan");
            temp.Add("utah");
            test.ReplaceDependees("world", temp);

            String dependees = "";
            IEnumerable values = test.GetDependees("world");
            foreach (String s in values)
            {
                dependees += s;
            }
            Assert.AreEqual("tomscottbrendanutah", dependees);
        }

        [TestMethod]
        public void Replace4()
        {
            DependencyGraph test = new DependencyGraph();
            test.AddDependency("hello", "world");
            test.AddDependency("cs", "3500");
            test.AddDependency("hello", "you");
            test.AddDependency("cs", "3810");
            test.AddDependency("cs", "2420");

            List<String> temp = new List<String>();
            temp.Add("tom");
            temp.Add("scott");
            temp.Add("brendan");
            temp.Add("utah");
            test.ReplaceDependees("world", temp);

            String dependents = "";
            IEnumerable values = test.GetDependents("cs");
            foreach (String s in values)
            {
                dependents += s;
            }
            Assert.AreEqual("350038102420", dependents);
        }

        [TestMethod]
        public void StressTest1()
        {
            DependencyGraph test = new DependencyGraph();
                String i = "hello";
                String temp = "";
                for (int j = 100000; j > 0; j--)
                {
                    temp += j;
                    test.AddDependency(i, j.ToString());
                }
            String dependents = "";
            IEnumerable values = test.GetDependents("hello");
            foreach (String s in values) 
            {
                dependents += s;
            }
            Assert.AreEqual(temp, dependents);
            
        }
    }
}
