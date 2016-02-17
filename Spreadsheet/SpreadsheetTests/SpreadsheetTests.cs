using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SS
{
    [TestClass]
    public class SpreadsheetTests : Spreadsheet
    {
        /// <summary>
        /// Test GetCellContents with null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContents1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }

        /// <summary>
        /// Test GetCellContents with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("hello");
        }

        /// <summary>
        /// Test GetCellContents with string
        /// </summary>
        [TestMethod]
        public void TestGetCellContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a2", "testing");
            Assert.AreEqual("testing", sheet.GetCellContents("a2"));
        }

        /// <summary>
        /// Test GetCellContents with double
        /// </summary>
        [TestMethod]
        public void TestGetCellContents4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a2", 2.5);
            Assert.AreEqual(2.5, sheet.GetCellContents("a2"));
        }

        /// <summary>
        /// Test GetCellContents with formula
        /// </summary>
        [TestMethod]
        public void TestGetCellContents5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a2", "2+5+x41+y23");
            Assert.AreEqual(new Formulas.Formula("2+5+x41+y23").ToString(), sheet.GetCellContents("a2"));
        }

        /// <summary>
        /// Test GetNamesOfAllNonemptyCells
        /// </summary>
        [TestMethod]
        public void TestGetNames1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", "a-one");
            sheet.SetCellContents("a2", "a-two");
            sheet.SetCellContents("a3", "a-three");
            String s = "";
            foreach(string t in sheet.GetNamesOfAllNonemptyCells())
            {
                s = s + t;
            }
            Assert.AreEqual("a1a2a3",s);
        }

        /// <summary>
        /// Test SetCellContents with null argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContents1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", null);
        }

        /// <summary>
        /// Test SetCellContents with null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null,"test");
        }

        /// <summary>
        /// Test SetCellContents with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("test", "test");
        }

        /// <summary>
        /// Test SetCellContents return dependents
        /// </summary>
        [TestMethod]
        public void SetCellContents4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formulas.Formula f1 = new Formulas.Formula("13+a1");
            Formulas.Formula f2 = new Formulas.Formula("a2+a1");
            sheet.SetCellContents("a1", 45);
            sheet.SetCellContents("a2", f1);
            sheet.SetCellContents("a3", f2);
            string s = "";
            foreach(string t in sheet.SetCellContents("a1", "12"))
            {
                s = s + t;
            }
            Assert.AreEqual("a1a2a3", s);
        }

        /// <summary>
        /// Test SetCellContents circular exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContents5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formulas.Formula f1 = new Formulas.Formula("13+a1");
            Formulas.Formula f2 = new Formulas.Formula("12+a2");
            sheet.SetCellContents("a1", "45");
            sheet.SetCellContents("a2", f1);
            sheet.SetCellContents("a1", f2);
        }

        /// <summary>
        /// Test GetCellContents on empty spreadsheet
        /// </summary>
        [TestMethod]
        public void GetCellContents6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// Test GetDirectDependents with null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDirectDependents1()
        {
            GetDirectDependents(null);
        }

        /// <summary>
        /// Test GetDirectDependents with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetDirectDependents2()
        {
            GetDirectDependents("test");
        }

        /// <summary>
        /// Test SetCellContents for formula with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, new Formulas.Formula("1+2"));
        }

        /// <summary>
        /// Test SetCellContents for double with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContents8()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("test", 2.5);
        }
    }
}
