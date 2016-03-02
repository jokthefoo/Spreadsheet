using System;
using System.Collections.Generic;
using Formulas;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Schema;
using System.Xml;

namespace SS
{
    /// <summary>
    /// A Spreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of a regular expression (called IsValid below) and an infinite 
    /// number of named cells.
    /// 
    /// A string is a valid cell name if and only if (1) s consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits AND (2) the C#
    /// expression IsValid.IsMatch(s.ToUpper()) is true.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are valid cell names, so long as they also
    /// are accepted by IsValid.  On the other hand, "Z", "X07", and "hello" are not valid cell 
    /// names, regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized by converting all letters to upper case before it is used by this 
    /// this spreadsheet.  For example, the Formula "x3+a5" should be normalize to "X3+A5" before 
    /// use.  Similarly, all cell names and Formulas that are returned or written to a file must also
    /// be normalized.
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
        /// <summary>
        /// Dictionary that maps cell names to cells
        /// </summary>
        private Dictionary<string, Cell> sheet;
        /// <summary>
        /// Dependency Graph of the spreadsheet to know which cells are dependent on other cells
        /// </summary>
        private Dependencies.DependencyGraph graph;
        /// <summary>
        /// Regex espression for the spreadsheet that you compare to, to see if things are valid
        /// </summary>
        private Regex isValid;

        private bool isChanged;
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return isChanged;
            }
            protected set
            {
                isChanged = value;
            }
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            sheet = new Dictionary<string, Cell>();
            graph = new Dependencies.DependencyGraph();
            isValid = new Regex("");
            Changed = false;
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        /// </summary>
        public Spreadsheet(Regex isVd)
        {
            sheet = new Dictionary<string, Cell>();
            graph = new Dependencies.DependencyGraph();
            isValid = isVd;
            Changed = false;
        }

        /// <summary>
        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  If there's a problem reading source, throws an IOException
        /// If the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  If there is an invalid cell name, or a 
        /// duplicate cell name, or an invalid formula in the source, throws a SpreadsheetReadException.
        /// If there's a Formula that causes a circular dependency, throws a SpreadsheetReadException. 
        /// </summary>
        public Spreadsheet(TextReader source)
        {
            sheet = new Dictionary<string, Cell>();
            graph = new Dependencies.DependencyGraph();

            XmlSchemaSet sc = new XmlSchemaSet();

            sc.Add(null, "Spreadsheet.xsd");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidationCallback;

            using (XmlReader reader = XmlReader.Create(source, settings))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                isValid = new Regex(reader["IsValid"]);
                                break;
                            case "cell":
                                try
                                {
                                    if(sheet.ContainsKey(reader["name"]))
                                    {
                                        throw new InvalidDataException();
                                    }
                                    SetContentsOfCell(reader["name"], reader["contents"]);
                                }
                                catch(InvalidNameException)
                                {
                                    throw new SpreadsheetReadException("Invalid name in source file: " + reader["name"]);
                                }
                                catch (CircularException)
                                {
                                    throw new SpreadsheetReadException("Formula causes circular dependency: " + reader["contents"]);
                                }
                                catch(FormulaFormatException)
                                {
                                    throw new SpreadsheetReadException("Invalid formula in source file: " + reader["contents"]);
                                }catch(InvalidDataException)
                                {
                                    throw new SpreadsheetReadException("Source file has duplicate cell name: " + reader["name"]);
                                }
                                break;
                        }
                    }
                }
            }
            Changed = false;
        }

        /// <summary>
        /// Displays validation errors
        /// </summary>
        private static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            throw new SpreadsheetReadException("Error within source file.");
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if(name == null || !isValidName(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
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
        private bool isValidName(string s)
        {
            String pattern = @"^[a-zA-Z]+[1-9][0-9]*$";
            if(s  == null)
            {
                return false;
            }
            if(Regex.IsMatch(s,pattern))
            {
                if(isValid.IsMatch(s.ToUpper()))
                {
                    return true;
                }
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula.Equals(null))
            {
                throw new ArgumentNullException();
            }
            if (!isValidName(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            formula = new Formula(formula.ToString().ToUpper());
            if (!sheet.ContainsKey(name))
            {
                sheet.Add(name, new Cell(name, formula));
            }

            Dependencies.DependencyGraph tempGraph = graph;
            Cell c;
            HashSet<string> set = new HashSet<string>();

            try
            {
                graph.ReplaceDependees(name, formula.GetVariables());

                foreach (string s in GetCellsToRecalculate(name))
                {
                    set.Add(s);
                }
            }
            catch(CircularException)
            {
                graph = tempGraph;
                foreach (string s in GetCellsToRecalculate(name))
                {
                    set.Add(s);
                }
                return set;
            }

            if (sheet.TryGetValue(name, out c))
            {
                c.SetContents(formula);
                sheet[name] = c;
            }

            UpdateValue(set);

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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> set = new HashSet<string>();

            if (text == null)
            {
                throw new ArgumentNullException();
            }
            if (!isValidName(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (!sheet.ContainsKey(name))
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

            if (text == "")
            {
                sheet.Remove(name);
            }

            UpdateValue(set);

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
        protected override ISet<string> SetCellContents(string name, double number)
        {
            if (!isValidName(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
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

            UpdateValue(set);

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
            else if(!isValidName(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            return graph.GetDependents(name);
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the isvalid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            using (XmlWriter writer = XmlWriter.Create(dest))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet", "");
                writer.WriteAttributeString("IsValid", isValid.ToString());

                foreach (string s in sheet.Keys)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteAttributeString("name", s);
                    if(sheet[s].GetContents().GetType() == typeof(Formula))
                    {
                        writer.WriteAttributeString("contents", "="+sheet[s].GetContents().ToString());
                    }
                    else
                    {
                        writer.WriteAttributeString("contents", sheet[s].GetContents().ToString());
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            Changed = false;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if(name == null || !isValidName(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            Cell c;
            sheet.TryGetValue(name, out c);
            return c.GetValue();
        }


        /// <summary>
        /// Updates the values of cells, called whenever a cell is changed
        /// </summary>
        public void UpdateValue(HashSet<string> set)
        {
            Cell c;
            foreach (string s in set)
            {
                if (sheet.ContainsKey(s))
                {
                    if (sheet[s].GetContents().GetType() == typeof(Formula))
                    {
                        Formula f = (Formula)sheet[s].GetContents();
                        try
                        {
                            double d = f.Evaluate(CellLookup);
                            sheet.TryGetValue(s, out c);
                            c.SetValue(d);
                            sheet[s] = c;
                        }
                        catch (FormulaEvaluationException e)
                        {
                            sheet.TryGetValue(s, out c);
                            c.SetValue(new FormulaError(e.Message.ToString()));
                            sheet[s] = c;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lookup for formula evaluate, Looks up cells and if their value is a double returns the double else throws error
        /// </summary>
        public double CellLookup(string name)
        {
            if (sheet.ContainsKey(name))
            {
                if (sheet[name].GetValue().GetType() == typeof(double))
                {
                    return (double)sheet[name].GetValue();
                }
            }
            throw new FormulaEvaluationException(name);
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if(content == null)
            {
                throw new ArgumentNullException();
            }
            if(name == null || !isValidName(name))
            {
                throw new InvalidNameException();
            }

            Changed = true;
            double d;
            if(double.TryParse(content, out d))
            {
                return SetCellContents(name, d);
            }else if(content.StartsWith("="))
            {
                Formula f = new Formula(content.Substring(1), s => s.ToUpper(), isValidName);
                return SetCellContents(name, f);
            }
            else
            {
                return SetCellContents(name, content);
            }
        }
    }
}
