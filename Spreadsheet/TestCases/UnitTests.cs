using System;
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
            Formula f = new Formula("2e10*");
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
            Formula f = new Formula("(*)");
        }

        // Test point 8 
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct10()
        {
            Formula f = new Formula("2.5e9 + x5 / 17*");
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

        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("25/5");
            Assert.AreEqual(f.Evaluate(s => 0), 5.0, 1e-6);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate5()
        {
            Formula f = new Formula("20/0");
            f.Evaluate(s => { throw new ArgumentException(); });
        }

        [TestMethod]
        public void Evaluate6()
        {
            Formula f = new Formula("25-5");
            Assert.AreEqual(f.Evaluate(s => 0), 20.0, 1e-6);
        }

        [TestMethod]
        public void Evaluate7()
        {
            Formula f = new Formula("2.5e2 + 15 / 17");
            Assert.AreEqual(f.Evaluate(s => 0), 250.882353, 1e-6);
        }

        [TestMethod]
        public void Evaluate8()
        {
            Formula f = new Formula("20 - 15 / 17");
            Assert.AreEqual(f.Evaluate(s => 0), 19.1176471, 1e-6);
        }

        [TestMethod]
        public void Evaluate9()
        {
            Formula f = new Formula("20 - (40/2)");
            Assert.AreEqual(f.Evaluate(s => 0), 0, 1e-6);
        }

        [TestMethod]
        public void Evaluate10()
        {
            Formula f = new Formula("20 - (x3 / 2)");
            Assert.AreEqual(f.Evaluate(s => 40), 0, 1e-6);
        }


        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct11()
        {
            Formula f = new Formula(")");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct12()
        {
            Formula f = new Formula("(2-3))");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct13()
        {
            Formula f = new Formula("");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct14()
        {
            Formula f = new Formula("+7");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct15()
        {
            Formula f = new Formula("((7-2)");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct16()
        {
            Formula f = new Formula("2 / 1 + 5-");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct17()
        {
            Formula f = new Formula("x");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct18()
        {
            Formula f = new Formula("-33");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct19()
        {
            Formula f = new Formula("-a1");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct20()
        {
            Formula f = new Formula("( 2 + 4) ( 5 -2 )");
        }
        
        [TestMethod]
        public void Evaluate11()
        {
            Formula f = new Formula("2-3");
            Assert.AreEqual(f.Evaluate(s => 0), -1.0, 1e-6);
        }
        [TestMethod]
        public void Evaluate12()
        {
            Formula f = new Formula("2*3");
            Assert.AreEqual(f.Evaluate(s => 0), 6.0, 1e-6);
        }
        [TestMethod]
        public void Evaluate13()
        {
            Formula f = new Formula("3/2");
            Assert.AreEqual(f.Evaluate(s => 0), 1.5, 1e-6);
        }
        [TestMethod]
        public void Evaluate14()
        {
            Formula f = new Formula("3e3");
            Assert.AreEqual(f.Evaluate(s => 0), 3000, 1e-6);
        }
        [TestMethod]
        public void Evaluate15()
        {
            Formula f = new Formula("(3 + 3) - 7 * (6/2)");
            Assert.AreEqual(f.Evaluate(s => 0), -15, 1e-6);
        }
        [TestMethod]
        public void Evaluate16()
        {
            Formula f = new Formula("(((aa1 + bb23) - cc3) * de19)");
            Assert.AreEqual(f.Evaluate(s => 1), 1, 1e-6);
        }
        [TestMethod]
        public void Evaluate17()
        {
            Formula f = new Formula("a2 + 4 - a8+ 3");
            Assert.AreEqual(f.Evaluate(s => 1), 7, 1e-6);
        }
        [TestMethod]
        public void Evaluate18()
        {
            Formula f = new Formula("600 / (4e2 - 2e2)");
            Assert.AreEqual(f.Evaluate(s => 1), 3, 1e-6);
        }
    }
}
