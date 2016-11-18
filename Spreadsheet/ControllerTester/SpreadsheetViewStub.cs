using SSGui;
using System;
using System.Windows.Forms;

namespace ControllerTester
{
    class SpreadsheetViewStub : ISpreadsheetView
    {

        // These two properties record whether a method has been called
        public bool CalledDoClose
        {
            get; private set;
        }

        public bool CalledOpenNew
        {
            get; private set;
        }

        // Properties implementing interface
        public string CellName
        {
            set; get;
        }

        public string Contents
        {
            set; get;
        }

        public string Message
        {
            set; get;
        }

        public string Name
        {
            set; get;
        }

        public SpreadsheetPanel spreadSheetPanel
        {
            set; get;
        }

        public string Title
        {
            set; get;
        }

        public string Value
        {
            set; get;
        }

        // Methods that fire the events
        public void FireCloseEvent()
        {
            if(CloseEvent != null)
            {
                CloseEvent();
            }
        }

        public void FireContentsEvent()
        {
            if (ContentsEvent != null)
            {
                ContentsEvent();
            }
        }

        public void FireFileChosenEvent(string filename)
        {
            if (FileChosenEvent != null)
            {
                FileChosenEvent(filename);
            }
        }

        public void FireFileSaveEvent(string filename)
        {
            if (FileSaveEvent != null)
            {
                FileSaveEvent(filename);
            }
        }

        public void FireHelpEvent()
        {
            if (HelpEvent != null)
            {
                HelpEvent();
            }
        }

        public void FireNewEvent()
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        public void FireXCloseEvent(FormClosingEventArgs e)
        {
            if (XCloseEvent != null)
            {
                XCloseEvent(e);
            }
        }

        public void FireSelectionEvent(SpreadsheetPanel s)
        {
            if (SelectionEvent != null)
            {
                SelectionEvent(s);
            }
        }

        // Events implementing interface
        public event Action CloseEvent;
        public event Action ContentsEvent;
        public event Action<string> FileChosenEvent;
        public event Action<string> FileSaveEvent;
        public event Action HelpEvent;
        public event Action NewEvent;
        public event Action<FormClosingEventArgs> XCloseEvent;
        public event Action<SpreadsheetPanel> SelectionEvent;

        // Methods implementing interface
        public DialogResult CloseMessage(string s)
        {
            return DialogResult.Yes;
        }

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }
    }
}
