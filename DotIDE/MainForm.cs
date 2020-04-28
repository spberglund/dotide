using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;
using System.Reflection;
using System.Windows.Resources;

namespace DotIDE
{
    public partial class MainForm : Form
    {
        TextEditor dotEditor;
        DotCompletionEngine dotEngine;

        public MainForm()
        {
            InitializeComponent();

            dotEditor = new TextEditor();     
            SearchPanel.Install(dotEditor);

            dotEditor.Text = (new StreamReader(Utils.GetFileStream("defaultDot.gv"))).ReadToEnd();

            ElementHost eHost = new ElementHost();
            eHost.Dock = DockStyle.Fill;
            eHost.Child = dotEditor;
            eHost.Name = "DotEditor";

            splitContainerText.Panel1.Controls.Add(eHost);

            dotEngine = new DotCompletionEngine(dotEditor, this);

            ToolStripMenuItemRecent.DropDownOpening += recentToolStripMenuItem_DropDownOpening;

            OpenRecent();

            dotEngine.CompileDot();
            dotEngine.autoCompileEnabled = true;
        }

        private void DownloadDot()
        {
            Process.Start("https://graphviz.gitlab.io/_pages/Download/windows/graphviz-2.38.msi");
        }
        private void NotYet()
        {
            MessageBox.Show("Can't do that yet, sarry");
        }
        private void AddToRecent(string file)
        {
            var rf = Properties.Settings.Default.recentFiles ?? new StringCollection();
            rf.Remove(file);
            rf.Insert(0, file);
            if (rf.Count > Properties.Settings.Default.maxRecent)
                rf.RemoveAt(Properties.Settings.Default.maxRecent);
            Properties.Settings.Default.recentFiles = rf;
            Properties.Settings.Default.Save();
        }

        private void OpenRecent(int index=0)
        {
            var rf = Properties.Settings.Default.recentFiles ?? new StringCollection();
            if (index < rf.Count) {
                dotEngine.Open(rf[index]);
                Text = string.Format("{0} {1}", Application.ProductName, rf[index]);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = Ui.OpenFile("Select a file to open", "gv", "txt", "dot", "xdot");
            if (File.Exists(file)) {
                dotEngine.Open(file);
                Text = string.Format("{0} {1}", Application.ProductName, file);
            }
        }

        public void UpdateVisualGraph(string fileName)
        {
            this.Invoke((MethodInvoker)(() => webBrowserGraph.Url = new Uri(fileName)));
        }

        public void ClearConsole()
        {
            this.Invoke((MethodInvoker)(() => textBoxConsole.Clear()));
        }
        public void WriteToConsole(string text)
        {
            this.Invoke((MethodInvoker)(() => textBoxConsole.AppendDotText(text)));
        }

        public void CompileFinished(object s, EventArgs e)
        {
            if (((Process)s).ExitCode == 0)
            {
                UpdateVisualGraph(dotEngine.graphFile);
                ClearConsole();
            }
            WriteToConsole(((Process)s).StandardOutput.ReadToEnd());
            WriteToConsole(((Process)s).StandardError.ReadToEnd());
        }
        private void installDotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadDot();
        }

        private void dotLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.graphviz.org/doc/info/lang.html");
        }

        private void dotAttributesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.graphviz.org/doc/info/attrs.html");
        }

        private void swapGraphTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotYet();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new About()) {
                about.ShowDialog();
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.OpenSettingsDialog() == DialogResult.OK)
                dotEngine.RefreshFileNames();
            dotEngine.CompileDot();
        }

        private void recentToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var rf = Properties.Settings.Default.recentFiles ?? new StringCollection();
            ToolStripMenuItemRecent.DropDown.Items.Clear();
            if (rf.Count > 0)
            {
                foreach (var file in rf)
                {
                    ToolStripMenuItemRecent.DropDown.Items.Add(file, null, (object sndr, EventArgs ea) => {
                        dotEngine.Open(((ToolStripItem)sndr).Text);
                        Text = string.Format("{0} {1}", Application.ProductName, file);
                    });
                }
            }
            else
            {
                ToolStripMenuItemRecent.DropDown.Items.Add("Empty");
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender=null, EventArgs e=null)
        {
            string file = Ui.SaveFile("What would you like to save this as?", dotEngine.dotSaveFile, "gv");
            if (!string.IsNullOrWhiteSpace(file))
            {
                dotEngine.Save(file);
                Text = string.Format("{0} {1}", Application.ProductName, file);
                AddToRecent(file);
            }
        }

        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(dotEngine.dotSaveFile)) dotEngine.Save();
            else saveAsToolStripMenuItem_Click();
        }
    }
}
