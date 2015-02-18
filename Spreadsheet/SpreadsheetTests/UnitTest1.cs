using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using SS;
using Formulas;

namespace SpreadsheetTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SpreadsheetCreation1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
        }

        [TestMethod]
        public void GetNamesNonemptyCells1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            IEnumerable names = sheet.GetNamesOfAllNonemptyCells();
            HashSet<String> cellNames = new HashSet<String>();
            foreach (String temp in names)
            {
                cellNames.Add(temp);
            }

            Assert.AreEqual(0, cellNames.Count);
        }

        [TestMethod]
        public void GetNamesNonemptyCells2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 23);
            sheet.SetCellContents("A2", 543);
            Formula f = new Formula("A1 + A2");
            sheet.SetCellContents("A3", f);
            sheet.SetCellContents("A4", "Testing");

            IEnumerable names = sheet.GetNamesOfAllNonemptyCells();
            HashSet<String> cellNames = new HashSet<String>();
            foreach (String temp in names)
            {
                cellNames.Add(temp);
            }

            Assert.AreEqual(4, cellNames.Count);
        }

        [TestMethod]
        public void GetCellContents1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 23);
            sheet.SetCellContents("A2", 543);
            Formula f = new Formula("A1 + A2");
            sheet.SetCellContents("A3", f);
            sheet.SetCellContents("A4", "Testing");

            Object actualContents = sheet.GetCellContents("A1");
            Object expectedContents = 23.0;

            Assert.IsTrue(actualContents.Equals(expectedContents));
        }

        [TestMethod]
        public void GetCellContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 23);
            sheet.SetCellContents("A2", 543);
            Formula f = new Formula("A1 + A2");
            sheet.SetCellContents("A3", f);
            sheet.SetCellContents("A4", "Testing");

            Object actualContents = sheet.GetCellContents("A2");
            Object expectedContents = 543.0;

            Assert.IsTrue(actualContents.Equals(expectedContents));
        }

        [TestMethod]
        public void GetCellContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 23);
            sheet.SetCellContents("A2", 543);
            Formula f = new Formula("A1 + A2");
            sheet.SetCellContents("A3", f);
            sheet.SetCellContents("A4", "Testing");

            Object actualContents = sheet.GetCellContents("A3");
            Object expectedContents = f;

            Assert.IsTrue(actualContents.Equals(expectedContents));
        }

        [TestMethod]
        public void GetCellContents4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 23);
            sheet.SetCellContents("A2", 543);
            Formula f = new Formula("A1 + A2");
            sheet.SetCellContents("A3", f);
            sheet.SetCellContents("A4", "Testing");

            Object actualContents = sheet.GetCellContents("A4");
            Object expectedContents = "Testing";

            Assert.IsTrue(actualContents.Equals(expectedContents));
        }

        [TestMethod]
        public void GetCellContents5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 23);
            sheet.SetCellContents("A2", 543);
            Formula f = new Formula("A1 + A2");
            sheet.SetCellContents("A3", f);
            sheet.SetCellContents("A4", "Testing");

            Object actualContents = sheet.GetCellContents("A5");
            Object expectedContents = "";

            Assert.IsTrue(actualContents.Equals(expectedContents));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 23);
            sheet.SetCellContents("A2", 543);
            Formula f = new Formula("A1 + A2");
            sheet.SetCellContents("A3", f);
            sheet.SetCellContents("A4", "Testing");

            Object actualContents = sheet.GetCellContents("X07");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 23);
            sheet.SetCellContents("B2", 53);

            String name = null;
            Object actualContents = sheet.GetCellContents(name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringNumber1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("Z", 23);
        }

        [TestMethod]
        public void SetCellContentsStringNumber2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 2);
            IEnumerable dependents = sheet.SetCellContents("B1", 545);

            HashSet<String> actualCellNames = new HashSet<String>();
            foreach (String temp in dependents)
            {
                actualCellNames.Add(temp);
            }

            HashSet<String> expectedCellNames = new HashSet<String>();
            expectedCellNames.Add("B1");
           
            foreach (String expected in expectedCellNames)
            {
                Assert.IsTrue(actualCellNames.Contains(expected));
            }

            foreach (String actual in actualCellNames)
            {
                Assert.IsTrue(expectedCellNames.Contains(actual));
            }
        }

        [TestMethod]
        public void SetCellContentsStringNumber3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 2);
            Formula b1 = new Formula("A1");
            sheet.SetCellContents("B1", b1);
            IEnumerable dependents = sheet.SetCellContents("C1",102);

            HashSet<String> actualCellNames = new HashSet<String>();
            foreach (String temp in dependents)
            {
                actualCellNames.Add(temp);
            }

            HashSet<String> expectedCellNames = new HashSet<String>();
            expectedCellNames.Add("C1");

            foreach (String expected in expectedCellNames)
            {
                Assert.IsTrue(actualCellNames.Contains(expected));
            }

            foreach (String actual in actualCellNames)
            {
                Assert.IsTrue(expectedCellNames.Contains(actual));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringNumber4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 23);
            String name = null;
            sheet.SetCellContents(name, 53);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsStringText1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            String text = null;
            sheet.SetCellContents("A1", text);
          
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringText2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            String name = null;
            sheet.SetCellContents(name, "Hello");
        }

        [TestMethod]
        public void SetCellContentsStringText3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", "hello");
            IEnumerable dependents = sheet.SetCellContents("B1", "world");

            HashSet<String> actualCellNames = new HashSet<String>();
            foreach (String temp in dependents)
            {
                actualCellNames.Add(temp);
            }

            HashSet<String> expectedCellNames = new HashSet<String>();
            expectedCellNames.Add("B1");

            foreach (String expected in expectedCellNames)
            {
                Assert.IsTrue(actualCellNames.Contains(expected));
            }

            foreach (String actual in actualCellNames)
            {
                Assert.IsTrue(expectedCellNames.Contains(actual));
            }
        }

        [TestMethod]
        public void SetCellContentsStringText4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", "Hello");
            Formula b1 = new Formula("A1");
            sheet.SetCellContents("B1", b1);
            IEnumerable dependents = sheet.SetCellContents("C1", "world");

            HashSet<String> actualCellNames = new HashSet<String>();
            foreach (String temp in dependents)
            {
                actualCellNames.Add(temp);
            }

            HashSet<String> expectedCellNames = new HashSet<String>();
            expectedCellNames.Add("C1");

            foreach (String expected in expectedCellNames)
            {
                Assert.IsTrue(actualCellNames.Contains(expected));
            }

            foreach (String actual in actualCellNames)
            {
                Assert.IsTrue(expectedCellNames.Contains(actual));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringText5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", "Hello");
            sheet.SetCellContents("Z", "world");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringFormula1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula a1 = new Formula("2+5");
            sheet.SetCellContents("A1", a1);
            String name = null;
            sheet.SetCellContents(name, a1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsStringFormula2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula a1 = new Formula("2+5");
            sheet.SetCellContents("A1", a1);
            Formula n = null;
            sheet.SetCellContents("B1", n);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContentsStringFormula3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula a1 = new Formula("C1");
            sheet.SetCellContents("A1", a1);
            Formula b1 = new Formula("A1");
            sheet.SetCellContents("B1", b1);
            Formula c1 = new Formula("B1");
            sheet.SetCellContents("C1", c1);
        }

        [TestMethod]
        public void SetCellContentsStringFormula4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula b1 = new Formula("A1*2");
            sheet.SetCellContents("B1", b1);
            Formula c1 = new Formula("B1 + A1");
            sheet.SetCellContents("C1", c1);
            IEnumerable dependents = sheet.SetCellContents("A1", 5);

            HashSet<String> actualCellNames = new HashSet<String>();
            foreach (String temp in dependents)
            {
                actualCellNames.Add(temp);
            }

            HashSet<String> expectedCellNames = new HashSet<String>();
            expectedCellNames.Add("A1");
            expectedCellNames.Add("B1");
            expectedCellNames.Add("C1");

            foreach (String expected in expectedCellNames)
            {
                Assert.IsTrue(actualCellNames.Contains(expected));
            }

            foreach (String actual in actualCellNames)
            {
                Assert.IsTrue(expectedCellNames.Contains(actual));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsStringFormula5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula b1 = new Formula("A1*2");
            sheet.SetCellContents("B1", b1);
            sheet.SetCellContents("F01", b1);
        }

        [TestMethod]
        public void SetCellContentsStringFormula6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 2);
            Formula a1 = new Formula("A1");
            sheet.SetCellContents("B1", a1);
            Formula b1 = new Formula("B1");
            IEnumerable dependents = sheet.SetCellContents("C2", b1);

            HashSet<String> actualCellNames = new HashSet<String>();
            foreach (String temp in dependents)
            {
                actualCellNames.Add(temp);
            }

            HashSet<String> expectedCellNames = new HashSet<String>();
            expectedCellNames.Add("C2");
            expectedCellNames.Add("B1");
            expectedCellNames.Add("A1");

            foreach (String expected in expectedCellNames)
            {
                Assert.IsTrue(actualCellNames.Contains(expected));
            }

            foreach (String actual in actualCellNames)
            {
                Assert.IsTrue(expectedCellNames.Contains(actual));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetDirectDependents1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula b1 = new Formula("A1*2");
            sheet.SetCellContents("B1", b1);
            sheet.SetCellContents("F01", b1);

            
        }
    }
}
