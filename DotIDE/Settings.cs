using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
//using System.Configuration.Internal;
using System.Reflection;
using System.IO;
using System.IO.Ports;

namespace DotIDE
{
    /// <summary>
    /// Automaticly constructs a context-aware settings page to allow the user to change program settings.
    /// </summary>
    public partial class Settings : Form
    {
        public bool? NoErrors = null;
        public Settings()
        {
            InitializeComponent();
            GenSettingsUI();
        }
        public static DialogResult OpenSettingsDialog()
        {
            DialogResult sResult;
            using (var settingForm = new Settings())
            {
                sResult = settingForm.ShowDialog();
            }
            return sResult;
        }
        public bool SaveChanges()
        {
            bool noErr = true;
            try
            {
                foreach (TabPage tab in tabControl.TabPages)
                {
                    foreach (Control ctrl in tab.Controls[0].Controls)
                    {
                        if (ctrl.Tag is SettingsPropertyValue)
                        {
                            try { SetSetting((SettingsPropertyValue)ctrl.Tag, ctrl); }
                            catch { noErr = false; }
                        }
                    }
                }
                Properties.Settings.Default.Save();
                return noErr;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public object SetSetting(SettingsPropertyValue setting, Control ctrl)
        {
            PropertyInfo sPropInfo = Properties.Settings.Default.GetType().GetProperty(setting.Name);
            object newVal = setting.PropertyValue;
            if (ctrl is CheckBox)
            {
                newVal = ((CheckBox)ctrl).Checked;
            }
            else if (ctrl is TextBox || ctrl is RichTextBox)
            {
                newVal = ctrl.Text;
            }
            else if (ctrl is ComboBox)
            {
                newVal = ((ComboBox)ctrl).SelectedValue;
            }
            else if (ctrl is NumericUpDown)
            {
                newVal = ((NumericUpDown)ctrl).Value;
            }
            object setVal = Convert.ChangeType(newVal, setting.Property.PropertyType);
            sPropInfo.SetValue(Properties.Settings.Default, setVal, null);
            return setVal;
        }
        public void GenSettingsUI()
        {
            var settings = Properties.Settings.Default.PropertyValues;

            var settingsDict = new Dictionary<string, Dictionary<Tuple<string, string, string>, SettingsPropertyValue>>();
            foreach (SettingsPropertyValue setting in settings)
            {
                string desc;
                if ((desc = GetSettingDesc(setting.Property)) == null) { continue; }

                var catDesc = ParseCatagory(desc);

                if (!settingsDict.Keys.Contains(catDesc.Item1))
                {
                    settingsDict.Add(catDesc.Item1, new Dictionary<Tuple<string, string, string>, SettingsPropertyValue>());
                }
                settingsDict[catDesc.Item1].Add(catDesc, setting);
            }
            foreach (KeyValuePair<string, Dictionary<Tuple<string, string, string>, SettingsPropertyValue>> settingCat in settingsDict)
            {
                var newTab = GenPage(settingCat.Key, settingCat.Value);
                tabControl.Controls.Add(newTab);
            }
        }
        public Tuple<string, string, string> ParseCatagory(string desc)
        {
            string[] parts = desc.Split(new char[] { ':' }, 3, StringSplitOptions.RemoveEmptyEntries);
            Tuple<string, string, string> catDesc;
            if (parts.Length == 3)
            {
                catDesc = new Tuple<string, string, string>(parts[0], parts[1], parts[2]);
            }
            else if (parts.Length == 2)
            {
                catDesc = new Tuple<string, string, string>(parts[0], parts[1], "");
            }
            else
            {
                catDesc = new Tuple<string, string, string>("Unspecified", parts[0], "");
            }
            return catDesc;
        }
        public TabPage GenPage(string pageName, Dictionary<Tuple<string, string, string>, SettingsPropertyValue> settingDictionary)
        {
            int rowInd = 0;
            var tabPage = new TabPage(pageName);
            var table = GenTable(pageName, settingDictionary.Count + 1, 2);
            tabPage.Controls.Add(table);
            foreach (KeyValuePair<Tuple<string, string, string>, SettingsPropertyValue> setting in settingDictionary)
            {
                var settingLabel = new Label();
                settingLabel.Dock = DockStyle.Fill;
                settingLabel.Text = setting.Key.Item2;
                settingLabel.Name = "label" + setting.Value.Name;
                settingLabel.AutoSize = true;
                settingLabel.TextAlign = ContentAlignment.MiddleRight;
                table.Controls.Add(settingLabel, 0, rowInd);
                table.Controls.Add(GetSelectorCtrl(setting.Key, setting.Value), 1, rowInd);
                rowInd++;
            }
            return tabPage;
        }
        public TableLayoutPanel GenTable(string name, int rows, int columns)
        {
            var table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.ColumnCount = columns;
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            table.Location = new System.Drawing.Point(12, 12);
            table.Name = "table" + name;
            table.RowCount = rows;
            table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            table.TabIndex = 0;
            return table;
        }
        public static Control GetSelectorCtrl(Tuple<string, string, string> setDesc, SettingsPropertyValue setting)
        {
            Control control = null;
            Type sTyp = setting.Property.PropertyType;
            dynamic sVal = setting.PropertyValue;
            string sDefault = setting.Property.DefaultValue.ToString();
            if (sVal is bool)
            {
                var ctrl = new CheckBox();
                ctrl.Checked = sVal;
                ctrl.Enabled = !setting.Property.IsReadOnly;
                control = ctrl;
            }
            else if (sVal is Enum)
            {
                var ctrl = new ComboBox();
                ctrl.Items.AddRange(Enum.GetNames(sVal));
                ctrl.SelectedValue = Enum.GetName(sTyp, sVal);
                control = ctrl;
            }
            else if (sVal is int || sVal is uint || sVal is long || sVal is byte || sVal is sbyte || sVal is short || sVal is double || sVal is float || sVal is decimal)
            {
                var ctrl = new NumericUpDown();
                if (sVal is double || sVal is float || sVal is decimal) { ctrl.DecimalPlaces = 10; }
                else { ctrl.DecimalPlaces = 0; }

                try { ctrl.Maximum = Convert.ToDecimal(Utils.MaxVal(sVal)); }
                catch (OverflowException e) { ctrl.Maximum = decimal.MaxValue; }
                try { ctrl.Minimum = Convert.ToDecimal(Utils.MinVal(sVal)); }
                catch (OverflowException e) { ctrl.Minimum = decimal.MinValue; }

                ctrl.Value = Convert.ToDecimal(sVal);
                control = ctrl;
            }
            else if (sVal is string)
            {
                if (sDefault.Contains("\n"))
                {
                    var ctrl = new RichTextBox();
                    ctrl.Text = sVal;
                    control = ctrl;
                }
                else
                {
                    var ctrl = new TextBox();
                    ctrl.Text = sVal;
                    control = ctrl;

                    if (setDesc.Item3 == "path")
                    {
                        ctrl.AutoCompleteSource = AutoCompleteSource.FileSystem;
                    }
                    else if (setDesc.Item3 == "dir")
                    {
                        ctrl.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
                    }
                    else if (setDesc.Item3 == "url")
                    {
                        ctrl.AutoCompleteSource = AutoCompleteSource.AllUrl;
                    }
                    else if (setDesc.Item3 == "com")
                    {
                        ctrl.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        ctrl.AutoCompleteCustomSource.AddRange(SerialPort.GetPortNames());
                    }
                    ctrl.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }
            else
            {
                var ctrl = new TextBox();
                ctrl.Text = sVal.ToString();
                control = ctrl;
            }
            control.Enabled = !setting.Property.IsReadOnly;
            control.Dock = DockStyle.Fill;
            control.Tag = setting;
            return control;
        }
        public static string GetSettingDesc(SettingsProperty setting)
        {
            foreach (object attr in setting.Attributes.Values)
            {
                if (attr is SettingsDescriptionAttribute)
                {
                    return ((SettingsDescriptionAttribute)attr).Description;
                }
            }
            return null;
        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!SaveChanges())
            {
                MessageBox.Show("There was an error saving the settings.\nSome changes may not be saved.", "Saving Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NoErrors = false;
                this.DialogResult = DialogResult.Abort;
            }
            else
            {
                NoErrors = true;
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
