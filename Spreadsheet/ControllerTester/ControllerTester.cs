using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSGui;
using System.Windows.Forms;

namespace ControllerTester
{
    [TestClass]
    public class ControllerTester : ISpreadsheetView
    {
        [TestMethod]
        public void TestMethod1()
        {
            ControllerTester window = new ControllerTester();
            new Controller(window);
            if (CloseEvent != null)
            {
                CloseEvent();
            }
            if (XCloseEvent != null)
            {
                XCloseEvent(new FormClosingEventArgs(CloseReason.ApplicationExitCall, false));
            }
            FileChosenEvent("testing");
            NewEvent();
            //spreadSheetPanel.SelectionChanged();
            ContentsEvent();
            FileSaveEvent("test");
            HelpEvent();
        }

        public string CellName
        {
            set { }
        }

        public string Contents
        {
            get { return ""; }
            set { }
        }

        public string Message
        {
            set { }
        }

        public string Name
        {
            set { }
        }

        public SpreadsheetPanel spreadSheetPanel
        {
            get { return new SpreadsheetPanel(); }
        }

        public string Title
        {
            set { }
        }

        public string Value
        {
            set { }
        }

        public event Action CloseEvent;
        public event Action ContentsEvent;
        public event Action<string> FileChosenEvent;
        public event Action<string> FileSaveEvent;
        public event Action HelpEvent;
        public event Action NewEvent;
        public event Action<System.Windows.Forms.FormClosingEventArgs> XCloseEvent;

        public System.Windows.Forms.DialogResult CloseMessage(string s)
        {
            throw new NotImplementedException();
        }

        public void DoClose()
        {
            throw new NotImplementedException();
        }

        public void OpenNew()
        {
            throw new NotImplementedException();
        }
    }
}
