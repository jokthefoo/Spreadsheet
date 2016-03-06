using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSGui;
using System.Windows.Forms;

namespace ControllerTester
{
    [TestClass]
    public class ControllerTester
    {
        [TestMethod]
        public void TestCloseEvent()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
        }

        [TestMethod]
        public void TestNewEvent()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireNewEvent();
            Assert.IsTrue(stub.CalledOpenNew);
        }

        [TestMethod]
        public void TestHelpName()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireHelpEvent();
            Assert.IsTrue(stub.Message.Equals("Click on a cell to select it.\nEnter desired contents of cell into \"Contents\" box and press enter to apply."));
        }

        [TestMethod]
        public void TestContentsBox()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireContentsEvent();
            Assert.AreEqual("Cannot change cell contents to " + null + " because: " + "Value cannot be null.", stub.Message);
        }

        [TestMethod]
        public void TestContentsBox2()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireFileChosenEvent("../../testing1.ss");
            stub.Contents = "2";
            stub.FireContentsEvent();
            Assert.AreEqual("2", stub.Value);
        }

        [TestMethod]
        public void TestSaveAndTitle()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireFileSaveEvent("../../testing.ss");
            Assert.AreEqual(stub.Title, "../../testing.ss");
        }

        [TestMethod]
        public void TestOpenFile()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireFileChosenEvent("../../testing1.ss");
            Assert.AreEqual("../../testing1.ss",stub.Title);
            Assert.AreEqual(stub.Contents, "=A2");
        }

        [TestMethod]
        public void TestOpenFile1()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireFileChosenEvent("../../NotRealFile.ss");
            Assert.IsTrue(stub.Message.StartsWith("Unable"));
        }

        [TestMethod]
        public void TestXClose()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireFileChosenEvent("../../testing1.ss");
            stub.Contents = "2";
            stub.FireContentsEvent();
            FormClosingEventArgs e;
            e = new FormClosingEventArgs(CloseReason.UserClosing, true);
            stub.FireXCloseEvent(e);
            Assert.AreEqual(true, e.Cancel);
        }

        [TestMethod]
        public void TestSelectionChange()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            stub.spreadSheetPanel = new SpreadsheetPanel();
            Controller controller = new Controller(stub);
            stub.FireSelectionEvent(stub.spreadSheetPanel);
            Assert.AreEqual(null,stub.Name);
        }
    }
}
