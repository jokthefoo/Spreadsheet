using System;
using System.Collections.Generic;
using Formulas;
using System.Text.RegularExpressions;
using System.IO;

namespace SS
{
    /// <summary>
    /// An Spreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.  Cell names
    /// are not case sensitive.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are cell names.  (Note that 
    /// "A15" and "a15" name the same cell.)  On the other hand, "Z", "X07", and 
    /// "hello" are not cell names."
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In an empty spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
    /// The value of a Formula, of course, can depend on the values of variables.  The value 
    /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
    /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
    /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
    /// is a double, as specified in Formula.Evaluate.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> sheet;
        private Dependencies.DependencyGraph graph;

        public override bool Changed
        {
            get
            {
                throw new NotImplementedException();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Zero arguement constructor that creates an empty spreadsheet
        /// </summary>
        public Spreadsheet()
        {
            sheet = new Dictionary<string, Cell>();
            graph = new Dependencies.DependencyGraph();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if(name == null || !isValid(name))
            {
                throw new InvalidNameException();
            }

            Cell c;
            if(sheet.TryGetValue(name, out c))
            {
                return c.GetContents();
            }else
            {
                return "";
            }
        }

        /// <summary>
        /// Checks to see if the name is a valid cell name
        /// </summary>
        private bool isValid(string s)
        {
            String pattern = @"^[a-zA-Z]+[1-9][0-9]*$";
            if(s  == null)
            {
                return false;
            }
            if(Regex.IsMatch(s,pattern))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach(Cell c in sheet.Values)
            {
                yield return c.GetName();
            }
        }

        /// <summary>
        /// If formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula.Equals(null))
            {
                throw new ArgumentNullException();
            }
            if (!isValid(name))
            {
                throw new InvalidNameException();
            }
            if (!sheet.ContainsKey(name))
            {
                sheet.Add(name, new Cell(name, formula));
            }

            Cell c;
            if (sheet.TryGetValue(name, out c))
            {
                c.SetContents(formula);
                graph.ReplaceDependees(name, formula.GetVariables());
                sheet[name] = c;
            }

            HashSet<string> set = new HashSet<string>();
            foreach (string s in GetCellsToRecalculate(name))
            {
                set.Add(s);
            }
            
            return set;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> set = new HashSet<string>();

            if (text == null)
            {
                throw new ArgumentNullException();
            }
            if (!isValid(name))
            {
                throw new InvalidNameException();
            }
            if(!sheet.ContainsKey(name))
            {
                sheet.Add(name, new Cell(name, text));
            }

            foreach (string s in GetCellsToRecalculate(name))
            {
                set.Add(s);
            }

            Cell c;
            if (sheet.TryGetValue(name, out c))
            {
                c.SetContents(text);
                sheet[name] = c;
            }
            if(text == "")
            {
                sheet.Remove(name);
            }

            return set;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            if (!isValid(name))
            {
                throw new InvalidNameException();
            }
            if (!sheet.ContainsKey(name))
            {
                sheet.Add(name, new Cell(name, number));
            }

            HashSet<string> set = new HashSet<string>();
            foreach (string s in GetCellsToRecalculate(name))
            {
                set.Add(s);
            }

            Cell c;
            if (sheet.TryGetValue(name, out c))
            {
                c.SetContents(number);
                sheet[name] = c;
            }

            return set;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException();
            }
            else if(!isValid(name))
            {
                throw new InvalidNameException();
            }
            return graph.GetDependents(name);
        }

        public override void Save(TextWriter dest)
        {
            throw new NotImplementedException();
        }

        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            throw new NotImplementedException();
        }
    }
}
