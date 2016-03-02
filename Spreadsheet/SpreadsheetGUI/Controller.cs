using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SSGui
{
    class Controller
    {
        // The window being controlled
        private ISpreadsheetView window;

        // The model being used
        private AbstractSpreadsheet spreadsheet;

        // The currently selected cell
        private string activeCell;

        public Controller(ISpreadsheetView window)
        {
            this.window = window;
            this.spreadsheet = new Spreadsheet();
            activeCell = "A1";
            window.CloseEvent += HandleClose;
            window.FileChosenEvent += HandleFileChosen;
            window.NewEvent += HandleNew;
            window.spreadSheetPanel.SelectionChanged += HandleSelectionChanged;
            window.ContentsEvent += HandleContentsBoxChange;
            window.FileSaveEvent += HandleSaveChosen;
        }

        /// <summary>
        /// Handles when the contents box is changed
        /// </summary>
        private void HandleContentsBoxChange()
        {
            try
            {
                spreadsheet.SetContentsOfCell(activeCell, window.Contents);
                foreach (string s in spreadsheet.GetNamesOfAllNonemptyCells())
                {
                    setCell(s);
                }
                setCell(activeCell);
            }
            catch(Exception e)
            {
                window.Message = "Cannot change cell contents to " + window.Contents + " because: " + e.Message;
            }
            RefreshTextBoxes();
        }

        /// <summary>
        /// Handles when selection on the spreadsheet is changed
        /// </summary>
        private void HandleSelectionChanged(SpreadsheetPanel ss)
        {
            int col,row;
            string name;
            ss.GetSelection(out col, out row);
            name = (Convert.ToChar(col+97)).ToString().ToUpper() + (row+1);
            activeCell = name;
            RefreshTextBoxes();
        }

        /// <summary>
        /// Refreshes what is in the text boxes
        /// </summary>
        private void RefreshTextBoxes()
        {
            window.CellName = activeCell;
            window.Value = spreadsheet.GetCellValue(activeCell).ToString();
            if(spreadsheet.GetCellContents(activeCell).GetType() == typeof(Formulas.Formula))
            {
                window.Contents = "="+spreadsheet.GetCellContents(activeCell).ToString();
            }
            else
            {
                window.Contents = spreadsheet.GetCellContents(activeCell).ToString();
            }
        }

        /// <summary>
        /// Handles request to open file
        /// </summary>
        private void HandleFileChosen(string filename)
        {
            try
            {
                TextReader read = File.OpenText(filename);
                spreadsheet = new Spreadsheet(read);
                foreach(string s in spreadsheet.GetNamesOfAllNonemptyCells())
                {
                    setCell(s);
                }
                RefreshTextBoxes();
            }
            catch(Exception e)
            {
                window.Message = "Unable to opwn file\n" + e.Message;
            }
        }

        /// <summary>
        /// Handles save request
        /// </summary>
        private void HandleSaveChosen(string filename)
        {
            StreamWriter w = new StreamWriter(filename);
            spreadsheet.Save(w);
            w.Close();
        }

        /// <summary>
        /// Sets the given cell to show the value
        /// </summary>
        private void setCell(string name)
        {
            string letterPat = @"[a-zA-z]+";
            string numPat = @"[0-9]+";
            int col = 0;
            int row = 0;
            String pattern = String.Format("({0})|({1})",
                                            letterPat,numPat);
            foreach (string s in Regex.Split(name,pattern))
            {
                if(Regex.IsMatch(s,letterPat))
                {
                    col = char.Parse(s.ToLower())-97;
                }
                else if(Regex.IsMatch(s,numPat))
                {
                    row = int.Parse(s)-1;
                }
            }
            window.spreadSheetPanel.SetValue(col, row, spreadsheet.GetCellValue(name).ToString());
        }

        /// <summary>
        /// Handles request to close file
        /// </summary>
        private void HandleClose()
        {
            if(spreadsheet.Changed == true)
            {
                window.Message = "File has been modified, do you want to save before closing?";
            }
            window.DoClose();
        }
        
        /// <summary>
        /// Handles request to open new window
        /// </summary>
        public void HandleNew()
        {
            window.OpenNew();
        }
    }
}
