using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace PS6WrittenTestCases
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EmptySheet1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void EmptySheet2()
        {
            Regex expression = new Regex(@"^=");
            AbstractSpreadsheet sheet = new Spreadsheet(expression);
            Assert.AreEqual("", sheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void GetNameNonEmptyCells1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("B53", "=5+2");
            sheet.SetContentsOfCell("C22", "HelloWorld");

            Assert.IsTrue(new HashSet<string>(sheet.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B53", "C22" }));
        }

       /* [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A11", "=A21+A4");
            sheet.SetContentsOfCell("A22", "23");
            sheet.GetCellContents(null);
        }*/

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("X07");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsofCell1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("X21", null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsofCell2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "x232");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsofCell3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "2");
            sheet.SetContentsOfCell("A2", "=A1");
            sheet.SetContentsOfCell("A1", "=A2");
        }

        [TestMethod]
        public void SetContentsofCell4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("X21", "2132");

            Assert.AreEqual(2132.0, sheet.GetCellValue("X21"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsofCell5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "242");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsofCell6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("X07", "242");
        }

        [TestMethod]
        public void Evaluation1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A11", "2");
            sheet.SetContentsOfCell("B11", "3");
            sheet.SetContentsOfCell("C11", "=A11+B11");

            Assert.AreEqual(5.0, sheet.GetCellValue("C11"));
        }

        [TestMethod]
        public void Evaluation2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A11", "2");
            sheet.SetContentsOfCell("B11", "0");
            sheet.SetContentsOfCell("C11", "=A11/B11");

            Object expected = new FormulaError();
           Assert.AreEqual(expected, sheet.GetCellValue("C11"));
        }

        [TestMethod]
        public void Evaluation3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A11", "2");
            sheet.SetContentsOfCell("B11", "5");
            sheet.SetContentsOfCell("A12", "10");
            sheet.SetContentsOfCell("B12", "20");
            sheet.SetContentsOfCell("C11", "=B11 * A11 + A12 + B12/2");

            Assert.AreEqual(30.0, sheet.GetCellValue("C11"));
        }

      /*  [TestMethod]
        public void Save1 ()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("X21", "2132");
            sheet.SetContentsOfCell("A22", "Testing123");
            sheet.SetContentsOfCell("B21", "=2+5");

            String xml = "";
           using (StreamWriter writer = File.CreateText("newfile.xml"))
           // using (StringWriter writer = new StringWriter())
            {
                sheet.Save(writer);
                xml = writer.ToString();
            }
           
            String expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?><spreadsheet isvalid=\"[\\s\\S]\"><cell><name>X21</name><contents>2132</contents></cell><cell><name>A22</name><contents>Testing123</contents></cell></spreadsheet>";

            Assert.AreEqual(expected, xml);
        }*/

        [TestMethod]
        public void Read1()
        {
            StreamReader reader = new StreamReader("newfile.xml");
            AbstractSpreadsheet sheet = new Spreadsheet(reader);

            Assert.AreEqual(2132.0, sheet.GetCellContents("X21"));
            Assert.AreEqual("Testing123", sheet.GetCellContents("A22"));
            Formula expected = new Formula("2+5");
            Formula actual = (Formula)sheet.GetCellContents("B21");

            String expectedAns = "2+5";
            String actualAns = actual.ToString();
            Assert.AreEqual(expectedAns, actualAns);
        }
    }
}
