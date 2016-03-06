using SS;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SSGui
{
    public class Controller
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
            spreadsheet = new Spreadsheet();
            activeCell = "A1";
            window.CloseEvent += HandleClose;
            window.XCloseEvent += XHandleClose;
            window.FileChosenEvent += HandleFileChosen;
            window.NewEvent += HandleNew;
            window.SelectionEvent += HandleSelectionChanged;
            window.ContentsEvent += HandleContentsBoxChange;
            window.FileSaveEvent += HandleSaveChosen;
            window.HelpEvent += HandleHelp;
        }

        /// <summary>
        /// Handles when help is clicked
        /// </summary>
        private void HandleHelp()
        {
            window.Message = "Click on a cell to select it.\nEnter desired contents of cell into \"Contents\" box and press enter to apply.";
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
            catch (Exception e)
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
            int col, row;
            string name;
            ss.GetSelection(out col, out row);
            name = (Convert.ToChar(col + 97)).ToString().ToUpper() + (row + 1);
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
            if (spreadsheet.GetCellContents(activeCell).GetType() == typeof(Formulas.Formula))
            {
                window.Contents = "=" + spreadsheet.GetCellContents(activeCell).ToString();
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
                window.Title = filename;
                ClearSheet();
                foreach (string s in spreadsheet.GetNamesOfAllNonemptyCells())
                {
                    setCell(s);
                }
                RefreshTextBoxes();
                read.Close();
            }
            catch (Exception e)
            {
                window.Message = "Unable to open file\n" + e.Message;
            }
        }

        /// <summary>
        /// Clears the spreadsheet
        /// </summary>
        private void ClearSheet()
        {
            string[] alph = { "A", "B","C","D","E","F","G","H","I","J","K","L"
                    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
            foreach (string s in alph)
            {
                for (int x = 1; x < 100; x++)
                {
                    setCell(s+x);
                }
            }
        }

        /// <summary>
        /// Handles save request
        /// </summary>
        private void HandleSaveChosen(string filename)
        {
            StreamWriter w = new StreamWriter(filename);
            window.Title = filename;
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
                                            letterPat, numPat);
            foreach (string s in Regex.Split(name, pattern))
            {
                if (Regex.IsMatch(s, letterPat))
                {
                    col = char.Parse(s.ToLower()) - 97;
                }
                else if (Regex.IsMatch(s, numPat))
                {
                    row = int.Parse(s) - 1;
                }
            }
            window.spreadSheetPanel.SetValue(col, row, spreadsheet.GetCellValue(name).ToString());
        }

        /// <summary>
        /// Handles request to close file
        /// </summary>
        private void HandleClose()
        {
            window.DoClose();
        }
        
        /// <summary>
        /// Handles request to close file
        /// </summary>
        private void XHandleClose(FormClosingEventArgs e)
        {
            if (spreadsheet.Changed == true)
            {
                if (DialogResult.Yes == window.CloseMessage("File has been modified, do you want to save before closing?"))
                {
                    e.Cancel = true;
                }
            }
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
