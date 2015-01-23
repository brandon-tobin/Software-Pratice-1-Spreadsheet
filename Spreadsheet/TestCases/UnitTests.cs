﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace TestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended primarily to show you
    /// how to create your own, which we strong recommend that you do!  To run them, pull down
    /// the Test menu and do Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("x");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        // Test point 2 
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula("");
        }

        // Test point 3
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5()
        {
            Formula f = new Formula("(x3)) + 2");
        }

        // Test point 4
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct6()
        {
            Formula f = new Formula("((x3) + 2");
        }

        // Test point 5 
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct7()
        {

        }

        // Test point 6 
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct8()
        {
            Formula f = new Formula("((x3) + 2)*");
        }

        // Test point 7
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct9()
        {

        }

        // Test point 8 
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct10()
        {

        }

        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(s => 0), 5.0, 1e-6);
        }

        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(s => 22.5), 22.5, 1e-6);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x5 + y6");
            f.Evaluate(s => { throw new ArgumentException(); });
        }
    }
}
