namespace DotIDE
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainerIDE = new System.Windows.Forms.SplitContainer();
            this.splitContainerText = new System.Windows.Forms.SplitContainer();
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.webBrowserGraph = new System.Windows.Forms.WebBrowser();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.emptyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refactorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.swapGraphTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installDotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dotLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dotAttributesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerIDE)).BeginInit();
            this.splitContainerIDE.Panel1.SuspendLayout();
            this.splitContainerIDE.Panel2.SuspendLayout();
            this.splitContainerIDE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerText)).BeginInit();
            this.splitContainerText.Panel2.SuspendLayout();
            this.splitContainerText.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerIDE
            // 
            this.splitContainerIDE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerIDE.Location = new System.Drawing.Point(0, 24);
            this.splitContainerIDE.Name = "splitContainerIDE";
            // 
            // splitContainerIDE.Panel1
            // 
            this.splitContainerIDE.Panel1.Controls.Add(this.splitContainerText);
            // 
            // splitContainerIDE.Panel2
            // 
            this.splitContainerIDE.Panel2.Controls.Add(this.webBrowserGraph);
            this.splitContainerIDE.Size = new System.Drawing.Size(1101, 615);
            this.splitContainerIDE.SplitterDistance = 541;
            this.splitContainerIDE.TabIndex = 0;
            // 
            // splitContainerText
            // 
            this.splitContainerText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerText.Location = new System.Drawing.Point(0, 0);
            this.splitContainerText.Name = "splitContainerText";
            this.splitContainerText.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerText.Panel2
            // 
            this.splitContainerText.Panel2.Controls.Add(this.textBoxConsole);
            this.splitContainerText.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainerText.Size = new System.Drawing.Size(541, 615);
            this.splitContainerText.SplitterDistance = 448;
            this.splitContainerText.TabIndex = 0;
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.AcceptsReturn = true;
            this.textBoxConsole.AcceptsTab = true;
            this.textBoxConsole.BackColor = System.Drawing.Color.Black;
            this.textBoxConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxConsole.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxConsole.ForeColor = System.Drawing.Color.White;
            this.textBoxConsole.Location = new System.Drawing.Point(0, 0);
            this.textBoxConsole.Multiline = true;
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ReadOnly = true;
            this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxConsole.Size = new System.Drawing.Size(541, 163);
            this.textBoxConsole.TabIndex = 0;
            // 
            // webBrowserGraph
            // 
            this.webBrowserGraph.CausesValidation = false;
            this.webBrowserGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserGraph.Location = new System.Drawing.Point(0, 0);
            this.webBrowserGraph.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserGraph.Name = "webBrowserGraph";
            this.webBrowserGraph.ScriptErrorsSuppressed = true;
            this.webBrowserGraph.Size = new System.Drawing.Size(556, 615);
            this.webBrowserGraph.TabIndex = 0;
            this.webBrowserGraph.WebBrowserShortcutsEnabled = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.refactorToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1101, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.ToolStripMenuItemRecent});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click_1);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemRecent
            // 
            this.ToolStripMenuItemRecent.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyToolStripMenuItem});
            this.ToolStripMenuItemRecent.Name = "ToolStripMenuItemRecent";
            this.ToolStripMenuItemRecent.Size = new System.Drawing.Size(186, 22);
            this.ToolStripMenuItemRecent.Text = "Recent";
            // 
            // emptyToolStripMenuItem
            // 
            this.emptyToolStripMenuItem.Name = "emptyToolStripMenuItem";
            this.emptyToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.emptyToolStripMenuItem.Text = "Empty";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.toolsToolStripMenuItem.Text = "Options";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // refactorToolStripMenuItem
            // 
            this.refactorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.swapGraphTypeToolStripMenuItem});
            this.refactorToolStripMenuItem.Name = "refactorToolStripMenuItem";
            this.refactorToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.refactorToolStripMenuItem.Text = "Refactor";
            // 
            // swapGraphTypeToolStripMenuItem
            // 
            this.swapGraphTypeToolStripMenuItem.Name = "swapGraphTypeToolStripMenuItem";
            this.swapGraphTypeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.swapGraphTypeToolStripMenuItem.Text = "Swap Graph Type";
            this.swapGraphTypeToolStripMenuItem.Click += new System.EventHandler(this.swapGraphTypeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installDotToolStripMenuItem,
            this.dotLanguageToolStripMenuItem,
            this.dotAttributesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // installDotToolStripMenuItem
            // 
            this.installDotToolStripMenuItem.Name = "installDotToolStripMenuItem";
            this.installDotToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.installDotToolStripMenuItem.Text = "Install Dot";
            this.installDotToolStripMenuItem.Click += new System.EventHandler(this.installDotToolStripMenuItem_Click);
            // 
            // dotLanguageToolStripMenuItem
            // 
            this.dotLanguageToolStripMenuItem.Name = "dotLanguageToolStripMenuItem";
            this.dotLanguageToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.dotLanguageToolStripMenuItem.Text = "Dot Language";
            this.dotLanguageToolStripMenuItem.Click += new System.EventHandler(this.dotLanguageToolStripMenuItem_Click);
            // 
            // dotAttributesToolStripMenuItem
            // 
            this.dotAttributesToolStripMenuItem.Name = "dotAttributesToolStripMenuItem";
            this.dotAttributesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.dotAttributesToolStripMenuItem.Text = "Dot Attributes";
            this.dotAttributesToolStripMenuItem.Click += new System.EventHandler(this.dotAttributesToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 639);
            this.Controls.Add(this.splitContainerIDE);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "DotIDE";
            this.splitContainerIDE.Panel1.ResumeLayout(false);
            this.splitContainerIDE.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerIDE)).EndInit();
            this.splitContainerIDE.ResumeLayout(false);
            this.splitContainerText.Panel2.ResumeLayout(false);
            this.splitContainerText.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerText)).EndInit();
            this.splitContainerText.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerIDE;
        private System.Windows.Forms.WebBrowser webBrowserGraph;
        private System.Windows.Forms.SplitContainer splitContainerText;
        private System.Windows.Forms.TextBox textBoxConsole;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refactorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem swapGraphTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installDotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dotLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dotAttributesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRecent;
        private System.Windows.Forms.ToolStripMenuItem emptyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}