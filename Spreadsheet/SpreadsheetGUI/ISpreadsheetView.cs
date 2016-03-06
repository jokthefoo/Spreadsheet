using System;
using System.Windows.Forms;

namespace SSGui
{
    public interface ISpreadsheetView
    {
        /// <summary>
        /// Fires when a file is chosen to be opened
        /// </summary>
        event Action<string> FileChosenEvent;
        /// <summary>
        /// Handles when close button is pressed
        /// </summary>
        event Action CloseEvent;
        /// <summary>
        /// Handles when red X is clicked
        /// </summary>
        event Action<FormClosingEventArgs> XCloseEvent;
        /// <summary>
        /// Handles when new is clicked
        /// </summary>
        event Action NewEvent;
        /// <summary>
        /// Handles when help is clicked
        /// </summary>
        event Action HelpEvent;
        /// <summary>
        /// Handles when save is clicked
        /// </summary>
        event Action<string> FileSaveEvent;
        /// <summary>
        /// Handles when contents box is changed
        /// </summary>
        event Action ContentsEvent;
        /// <summary>
        /// Handles selection of a cell event
        /// </summary>
        event Action<SpreadsheetPanel> SelectionEvent;

        string Name { set; }
        /// <summary>
        /// Current cell name textbox
        /// </summary>
        string CellName { set; }
        /// <summary>
        /// Current cell value box
        /// </summary>
        string Value { set; }
        /// <summary>
        /// Current cell contents box
        /// </summary>
        string Contents { set; get; }
        /// <summary>
        /// Title of window
        /// </summary>
        string Title { set; }
        /// <summary>
        /// Popup messages
        /// </summary>
        string Message { set; }
        /// <summary>
        /// The spreadsheet panel
        /// </summary>
        SpreadsheetPanel spreadSheetPanel { get; }
        /// <summary>
        /// Close
        /// </summary>
        void DoClose();
        /// <summary>
        /// Open new
        /// </summary>
        void OpenNew();
        /// <summary>
        /// Close message box
        /// </summary>
        DialogResult CloseMessage(string s);
    }
}
