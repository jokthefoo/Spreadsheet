using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    struct Cell
    {
        /// <summary>
        /// Name of the cell
        /// </summary>
        private string Name;
        /// <summary>
        /// Value of the cell
        /// </summary>
        private object Value;
        /// <summary>
        /// Contents of the cell
        /// </summary>
        private object contents;

        /// <summary>
        /// Constructor for a cell containing a string
        /// </summary>
        public Cell(string n, string v)
        {
            Name = n;
            contents = v;
            Value = v;
        }

        /// <summary>
        /// Constructor for a cell containing a double
        /// </summary>
        public Cell(string n, double v)
        {
            Name = n;
            contents = v;
            Value = v;
        }

        /// <summary>
        /// Constructor for a cell containing a Formula
        /// </summary>
        public Cell(string n, Formulas.Formula v)
        {
            Name = n;
            contents = v;
            Value = new FormulaError();
        }


        /// <summary>
        /// Returns the contents of the cell
        /// </summary>
        public object GetContents()
        {
            return contents;
        }

        /// <summary>
        /// Returns the name of the cell
        /// </summary>
        public string GetName()
        {
            return Name;
        }

        /// <summary>
        /// Sets the contents of the cell
        /// </summary>
        public void SetContents(Formulas.Formula f)
        {
            contents = f;
        }

        /// <summary>
        /// Sets the contents of the cell
        /// </summary>
        public void SetContents(double d)
        {
            contents = d;
            Value = d;
        }

        /// <summary>
        /// Sets the contents of the cell
        /// </summary>
        public void SetContents(string s)
        {
            contents = s;
            Value = s;
        }

        /// <summary>
        /// Returns the value of the cell
        /// </summary>
        public object GetValue()
        {
            return Value;
        }


        /// <summary>
        /// Sets the value of the cell
        /// </summary>
        public void SetValue(double d)
        {
            Value = d;
        }

        /// <summary>
        /// Sets the value of the cell
        /// </summary>
        public void SetValue(string s)
        {
            Value = s;
        }

        /// <summary>
        /// Sets the value of the cell
        /// </summary>
        public void SetValue(FormulaError e)
        {
            Value = e;
        }
    }
}
