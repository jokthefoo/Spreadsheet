using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSGui
{
    interface ISpreadsheetView
    {
        event Action<string> FileChosenEvent;

        event Action CloseEvent;

        event Action NewEvent;

        event Action<string> FileSaveEvent;

        event Action ContentsEvent;

        string Name { set; }

        string CellName { set; }

        string Value { set; }

        string Contents { set; get; }

        string Title { set; }

        string Message { set; }

        SpreadsheetPanel spreadSheetPanel { get;}

        void DoClose();

        void OpenNew();
    }
}
