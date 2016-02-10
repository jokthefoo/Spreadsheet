// Written by Joe Zachary for CS 3500, January 2016.
// Repaired error in Evaluate5.  Added TestMethod Attribute
//    for Evaluate4 and Evaluate5 - JLZ January 25, 2016
// Corrected comment for Evaluate3 - JLZ January 29, 2016

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This is another syntax error I added
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MyConstruct1()
        {
            Formula f = new Formula("23(2.525y + y) / 2");
        }

        /// <summary>
        /// This is another syntax error I added testing first token validity
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MyConstruct5()
        {
            Formula f = new Formula("(x5 + y)) / 2");
        }

        /// <summary>
        /// This is another syntax error I added testing first token validity
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MyConstruct2()
        {
            Formula f = new Formula(")2+2");
        }

        /// <summary>
        /// This is another syntax error I added testing last token validity
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MyConstruct3()
        {
            Formula f = new Formula("2+2(");
        }

        /// <summary>
        /// This is another syntax error I added testing no tokens
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MyConstruct4()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This tests that a no argument constructor works as if you did new formula("0");
        /// </summary>
        [TestMethod]
        public void Construct0()
        {
            Formula f = new Formula();
            Assert.AreEqual(f.Evaluate(v => 0), 0, 1e-6);
        }

        /// <summary>
        /// This tests that a no argument constructor works as if you did new formula("0");
        /// </summary>
        [TestMethod]
        public void Construct01()
        {
            Formula f = new Formula(null);
            Assert.AreEqual(f.Evaluate(v => 0), 0, 1e-6);
        }

        /// <summary>
        /// This tests negative double
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula("-2.0+3x");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5)
        /// </summary>
        [TestMethod]
        public void MyEvaluate1()
        {
            Formula f = new Formula("x");
            Assert.AreEqual(f.Evaluate(Lookup4), 4.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a variable that doesn't exsist
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void MyEvaluate3()
        {
            Formula f = new Formula("x+y+z+f");
            Assert.AreEqual(f.Evaluate(Lookup4), 4.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5)
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void MyEvaluate2()
        {
            Formula f = new Formula("10 / 0");
            f.Evaluate(v => 0);
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        /// <summary>
        /// Tests ToString
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual("2+3", f.ToString());
        }

        /// <summary>
        /// Tests get variables
        /// </summary>
        [TestMethod]
        public void TestGetVariables()
        {
            Formula f = new Formula("c2+x3");
            String z = "";
            foreach(string s in f.GetVariables())
            {
                z = z + s;
            }
            Assert.AreEqual("c2x3", z);
        }

        /// <summary>
        /// This tests the 3 argument constructor
        /// </summary>
        [TestMethod]
        public void NormalizeConstruct0()
        {
            Formula f = new Formula("x3+t5",N,V);
        }

        /// <summary>
        /// Normalizer used for testing, Converts letters to uppercase
        /// </summary>
        public string N(string s)
        {
            return s.ToUpper();
        }

        /// <summary>
        /// Validator used for testing, Variables are only valid if they are a letter followed by a number
        /// </summary>
        public bool V(string s)
        {
            String varPattern = @"[a-zA-Z][0-9]";
            if (s.Length == 2)
            {
                if (Regex.IsMatch(s, varPattern))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This tests the 3 argument constructor when the Normalizer makes a variable invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void NormalizeConstruct1()
        {
            Formula f = new Formula("x3+t5", N1, V);
        }

        /// <summary>
        /// Normalizer used for testing, adds a number at the start of the variable to make it invalid
        /// </summary>
        public string N1(string s)
        {
            s = "&^%&";
            return s;
        }


        /// <summary>
        /// This tests the 3 argument constructor when the Normalizer makes a variable invalid according to the Validator
        /// The normalizer makes the Variables upper case but the validator requires lower case variables.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void NormalizeConstruct2()
        {
            Formula f = new Formula("x3+t5", N, V1);
        }

        /// <summary>
        /// Validator used for testing, Variables are only valid if they are a lower case letter followed by a number
        /// </summary>
        public bool V1(string s)
        {
            String varPattern = @"[a-z][0-9]";
            if (s.Length == 2)
            {
                if (Regex.IsMatch(s, varPattern))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
