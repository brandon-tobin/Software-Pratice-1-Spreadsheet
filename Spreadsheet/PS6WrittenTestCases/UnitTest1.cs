using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            sheet.SetContentsOfCell("A1", "=A2");
            sheet.SetContentsOfCell("A2", "=A1");
        }

        [TestMethod]
        public void SetContentsofCell4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("X21", "2132");

            Assert.AreEqual(2132.0, sheet.GetCellValue("X21"));
        }
    }
}
