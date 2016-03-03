using System;
using System.Windows.Forms;

namespace SSGui
{
    static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Get the application context and run one form inside it
            var context = SpreadsheetContext.GetContext();
            SpreadsheetContext.GetContext().RunNew();
            Application.Run(context);
        }
    }
}
