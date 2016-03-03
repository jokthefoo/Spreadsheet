namespace SSGui
{
    partial class SpreadsheetWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.spreadsheetPanel = new SSGui.SpreadsheetPanel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.openFile = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.closeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellNameTextBox = new System.Windows.Forms.TextBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.contentsTextBox = new System.Windows.Forms.TextBox();
            this.cellNameLabel = new System.Windows.Forms.Label();
            this.cellValueLabel = new System.Windows.Forms.Label();
            this.cellContentsLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.helpTool = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel
            // 
            this.spreadsheetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel.Location = new System.Drawing.Point(0, 28);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(1345, 890);
            this.spreadsheetPanel.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1345, 28);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newWindow,
            this.openFile,
            this.saveFile,
            this.closeItem,
            this.helpTool});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(44, 24);
            this.fileMenu.Text = "File";
            // 
            // newWindow
            // 
            this.newWindow.Name = "newWindow";
            this.newWindow.Size = new System.Drawing.Size(181, 26);
            this.newWindow.Text = "New";
            this.newWindow.Click += new System.EventHandler(this.newWindow_Click);
            // 
            // openFile
            // 
            this.openFile.Name = "openFile";
            this.openFile.Size = new System.Drawing.Size(181, 26);
            this.openFile.Text = "Open...";
            this.openFile.Click += new System.EventHandler(this.openFile_Click);
            // 
            // saveFile
            // 
            this.saveFile.Name = "saveFile";
            this.saveFile.Size = new System.Drawing.Size(181, 26);
            this.saveFile.Text = "Save...";
            this.saveFile.Click += new System.EventHandler(this.saveFile_Click);
            // 
            // closeItem
            // 
            this.closeItem.Name = "closeItem";
            this.closeItem.Size = new System.Drawing.Size(181, 26);
            this.closeItem.Text = "Close";
            this.closeItem.Click += new System.EventHandler(this.closeItem_Click);
            // 
            // cellNameTextBox
            // 
            this.cellNameTextBox.Location = new System.Drawing.Point(213, 6);
            this.cellNameTextBox.Name = "cellNameTextBox";
            this.cellNameTextBox.ReadOnly = true;
            this.cellNameTextBox.Size = new System.Drawing.Size(49, 22);
            this.cellNameTextBox.TabIndex = 2;
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(347, 5);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.ReadOnly = true;
            this.valueTextBox.Size = new System.Drawing.Size(179, 22);
            this.valueTextBox.TabIndex = 3;
            // 
            // contentsTextBox
            // 
            this.contentsTextBox.Location = new System.Drawing.Point(637, 5);
            this.contentsTextBox.Name = "contentsTextBox";
            this.contentsTextBox.Size = new System.Drawing.Size(180, 22);
            this.contentsTextBox.TabIndex = 4;
            this.contentsTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.contentsTextBox_KeyUp);
            // 
            // cellNameLabel
            // 
            this.cellNameLabel.AutoSize = true;
            this.cellNameLabel.Location = new System.Drawing.Point(161, 10);
            this.cellNameLabel.Name = "cellNameLabel";
            this.cellNameLabel.Size = new System.Drawing.Size(49, 17);
            this.cellNameLabel.TabIndex = 5;
            this.cellNameLabel.Text = "Name:";
            // 
            // cellValueLabel
            // 
            this.cellValueLabel.AutoSize = true;
            this.cellValueLabel.Location = new System.Drawing.Point(293, 9);
            this.cellValueLabel.Name = "cellValueLabel";
            this.cellValueLabel.Size = new System.Drawing.Size(48, 17);
            this.cellValueLabel.TabIndex = 6;
            this.cellValueLabel.Text = "Value:";
            // 
            // cellContentsLabel
            // 
            this.cellContentsLabel.AutoSize = true;
            this.cellContentsLabel.Location = new System.Drawing.Point(563, 7);
            this.cellContentsLabel.Name = "cellContentsLabel";
            this.cellContentsLabel.Size = new System.Drawing.Size(68, 17);
            this.cellContentsLabel.TabIndex = 7;
            this.cellContentsLabel.Text = "Contents:";
            // 
            // helpTool
            // 
            this.helpTool.Name = "helpTool";
            this.helpTool.Size = new System.Drawing.Size(181, 26);
            this.helpTool.Text = "Help";
            this.helpTool.Click += new System.EventHandler(this.helpTool_Click);
            // 
            // SpreadsheetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1345, 918);
            this.Controls.Add(this.cellContentsLabel);
            this.Controls.Add(this.cellValueLabel);
            this.Controls.Add(this.cellNameLabel);
            this.Controls.Add(this.contentsTextBox);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.cellNameTextBox);
            this.Controls.Add(this.spreadsheetPanel);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "SpreadsheetWindow";
            this.Text = "Spreadsheet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetWindow_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SSGui.SpreadsheetPanel spreadsheetPanel;
        private System.Windows.Forms.TextBox cellNameTextBox;
        private System.Windows.Forms.Label cellNameLabel;
        private System.Windows.Forms.Label cellValueLabel;
        private System.Windows.Forms.Label cellContentsLabel;
        private System.Windows.Forms.TextBox contentsTextBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.ToolStripMenuItem closeItem;
        private System.Windows.Forms.ToolStripMenuItem saveFile;
        private System.Windows.Forms.ToolStripMenuItem openFile;
        private System.Windows.Forms.ToolStripMenuItem newWindow;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem helpTool;
    }
}

