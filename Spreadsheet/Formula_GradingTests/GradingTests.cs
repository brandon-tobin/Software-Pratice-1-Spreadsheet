using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Formulas;
using System.Text;

namespace GradingTests
{
    /// <summary>
    /// These tests use only the zero-argument constructor
    /// </summary>
    [TestClass]
    public class GradingTests_A
    {
        // Tests of syntax errors detected by the constructor
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test1a()
        {
            Formula f = new Formula("        ");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test2a()
        {
            Formula f = new Formula("((2 + 5))) + 8");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test3a()
        {
            Formula f = new Formula("2+5*8)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test5a()
        {
            Formula f = new Formula("+3");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test6a()
        {
            Formula f = new Formula("-y2");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test9a()
        {
            Formula f = new Formula(")");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test11a()
        {
            Formula f = new Formula("2 5");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test12a()
        {
            Formula f = new Formula("x5 y7");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test13a()
        {
            Formula f = new Formula("((((((((((2)))))))))");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test14a()
        {
            Formula f = new Formula("x_");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15a()
        {
            Formula f = new Formula("x5 + x6 + x7 + (x8) +");
        }

        // Simple tests that throw FormulaEvaluationExceptions
        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test16a()
        {
            Formula f = new Formula("2+X1");
            f.Evaluate(s => { throw new ArgumentException(); });
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test17a()
        {
            Formula f = new Formula("5/0");
            f.Evaluate(s => 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test18a()
        {
            Formula f = new Formula("(5 + X1) / (X1 - 3)");
            f.Evaluate(s => 3);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test19a()
        {
            Formula f = new Formula("x1 + x2");
            f.Evaluate(s => { if (s == "x1") return 0; else throw new ArgumentException(); });
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test20a()
        {
            Formula f = new Formula("x1 + x2 * x3 + x4 * x5 * x6 + x7");
            f.Evaluate(s => { if (s == "x7") throw new ArgumentException(); else return 1; });
        }

        // Simple formulas
        [TestMethod()]
        public void Test21a()
        {
            Formula f = new Formula("4.5e1");
            Assert.AreEqual(45, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test22a()
        {
            Formula f = new Formula("a0");
            Assert.AreEqual(10, f.Evaluate(s => 10), 1e-6);
        }

        [TestMethod()]
        public void Test24a()
        {
            Formula f = new Formula("5 - x6");
            Assert.AreEqual(1, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test25a()
        {
            Formula f = new Formula("5 * x6");
            Assert.AreEqual(20, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test26a()
        {
            Formula f = new Formula("8 / x6");
            Assert.AreEqual(2, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod()]
        public void Test28a()
        {
            Formula f = new Formula("1 + 2 + 3 * 4 + 5");
            Assert.AreEqual(20, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test29a()
        {
            Formula f = new Formula("(1 + 2 + 3 * 4 + 5) * 2");
            Assert.AreEqual(40, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod()]
        public void Test31a()
        {
            Formula f = new Formula("((((((((((((x7))))))))))))");
            Assert.AreEqual(7, f.Evaluate(s => 7), 1e-6);
        }

        // Some more complicated formula evaluations
        [TestMethod()]
        public void Test32a()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
        }

        [TestMethod()]
        public void Test33a()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod()]
        public void Test34a()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
        }

        [TestMethod()]
        public void Test35a()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
        }

        // Test to make sure there can be more than one formula at a time
        [TestMethod()]
        public void Test36a()
        {
            Formula f1 = new Formula("x5+3");
            Formula f2 = new Formula("x5-3");
            Assert.AreEqual(6, f1.Evaluate(s => 3), 1e-6);
            Assert.AreEqual(0, f2.Evaluate(s => 3), 1e-6);
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test41a()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }
    }


    /// <summary>
    /// These tests use mostly the 3-argument constructor
    /// </summary>
    [TestClass]
    public class GradingTests_B
    {
        // Tests of syntax errors detected by the constructor
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test1b()
        {
            Formula f = new Formula("((2 + 5))) + 8", s => s, s => true);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test2b()
        {
            Formula f = new Formula("x_", s => s, s => true);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test3b()
        {
            Formula f = new Formula("x1", s => "#", s => true);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test4b()
        {
            Formula f = new Formula("x1", s => "Y", s => s != "Y");
        }

        [TestMethod()]
        public void Test5b()
        {
            Formula f = new Formula("x1", s => "Y", s => s == "Y");
        }

        [TestMethod()]
        public void Test6b()
        {
            Formula f = new Formula("x1 + y2 + z3", s => s.ToUpper(), s => ('X' <= s[0] && s[0] <= 'Z'));
        }

        [TestMethod()]
        public void Test7b()
        {
            Formula f = new Formula("x1 + y2 + z3", s => s.ToUpper(), s => true);
            Assert.AreEqual(12.0, f.Evaluate(lookup7b), 1e-6);
        }

        [TestMethod()]
        public void Test8b()
        {
            Formula f = new Formula("X1 + Y2 + Z3", s => s, s => true);
            Assert.AreEqual(12.0, f.Evaluate(lookup7b), 1e-6);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test9b()
        {
            Formula f = new Formula("x1 + y2 + z4", s => s.ToUpper(), s => true);
            f.Evaluate(lookup7b);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test10b()
        {
            Formula f = new Formula("X1 + Y2 + Z4", s => s, s => true);
            f.Evaluate(lookup7b);
        }

        [TestMethod()]
        public void Test11b()
        {
            Formula f = new Formula("X1 + Y2 + Z3");
            Assert.AreEqual(12.0, f.Evaluate(lookup7b), 1e-6);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test12b()
        {
            Formula f = new Formula("X1 + Y2 + Z4");
            f.Evaluate(lookup7b);
        }

        private double lookup7b (string s)
        {
            switch (s)
            {
                case "X1":
                    return 3;
                case "Y2":
                    return 4;
                case "Z3":
                    return 5;
                default:
                    throw new FormulaEvaluationException("Unknown variable");
            }
        }

        [TestMethod()]
        public void Test13b()
        {
            Formula f = new Formula("2 * 7");
            List<string> result = new List<string>(f.GetVariables());
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod()]
        public void Test14b()
        {
            Formula f = new Formula("X1 + Y2 + Z3");
            List<string> result = new List<string>(f.GetVariables());
            Assert.AreEqual(3, result.Count);
            CollectionAssert.Contains(result, "X1");
            CollectionAssert.Contains(result, "Y2");
            CollectionAssert.Contains(result, "Z3");
        }

        [TestMethod()]
        public void Test15b()
        {
            Formula f = new Formula("X1 + Y2 + Z3 * Y2 + X1");
            List<string> result = new List<string>(f.GetVariables());
            Assert.AreEqual(3, result.Count);
            CollectionAssert.Contains(result, "X1");
            CollectionAssert.Contains(result, "Y2");
            CollectionAssert.Contains(result, "Z3");
        }

        [TestMethod()]
        public void Test16b()
        {
            Formula f = new Formula("x1 + y2 + z3", s => s.ToUpper(), s => true);
            List<string> result = new List<string>(f.GetVariables());
            Assert.AreEqual(3, result.Count);
            CollectionAssert.Contains(result, "X1");
            CollectionAssert.Contains(result, "Y2");
            CollectionAssert.Contains(result, "Z3");
        }

        [TestMethod()]
        public void Test17b()
        {
            Formula f = new Formula("x1 + y2 + z3 * y2 + x1", s => s.ToUpper(), s => true);
            List<string> result = new List<string>(f.GetVariables());
            Assert.AreEqual(3, result.Count);
            CollectionAssert.Contains(result, "X1");
            CollectionAssert.Contains(result, "Y2");
            CollectionAssert.Contains(result, "Z3");
        }

        [TestMethod()]
        public void Test18b()
        {
            Formula f = new Formula("x1 + y2 + z3 * Y2 + X1", s => s.ToUpper(), s => true);
            List<string> result = new List<string>(f.GetVariables());
            Assert.AreEqual(3, result.Count);
            CollectionAssert.Contains(result, "X1");
            CollectionAssert.Contains(result, "Y2");
            CollectionAssert.Contains(result, "Z3");
        }

        [TestMethod()]
        public void Test19b()
        {
            StringBuilder sb = new StringBuilder("a1");
            for (int i = 2; i <= 500000; i++)
            {
                sb.Append("+a" + i); 
            }
            Formula f = new Formula(sb.ToString(), s => "_", s => (s == "_"));
            Assert.AreEqual(500000, f.Evaluate(s => { if (s == "_") return 1; else throw new ArgumentException(); }));
            List<string> result = new List<string>(f.GetVariables());
            Assert.AreEqual(1, result.Count);
            CollectionAssert.Contains(result, "_");
        }

        [TestMethod()]
        public void Test20b()
        {
            Test19b();
        }
    }
}
