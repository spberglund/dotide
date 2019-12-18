using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace DotIDE
{
    public static class Ui
    {
        public static string OpenFile(string title, params string[] fileExts)
        {

            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Title = title;
                openDialog.Filter = GenFilterString(fileExts);
                openDialog.Multiselect = false;
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    return openDialog.FileName;
                }
                else
                {
                    return "";
                }
            }
        }

        public static string SaveFile(string title, string path, params string[] fileExts)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = title;
                try
                {
                    saveDialog.FileName = Path.GetFileName(path);
                    saveDialog.InitialDirectory = Path.GetDirectoryName(path);
                }
                catch { }
                saveDialog.Filter = GenFilterString(fileExts);
                saveDialog.DefaultExt = fileExts[0];
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveDialog.FileName;
                }
                else
                {
                    return "";
                }
            }
        }
        private static string GenFilterString(string[] fileExts)
        {
            var filters = new List<string>();
            foreach (string fileExt in fileExts)
            {
                filters.Add("{0} files (*.{1})|*{1}".Fmt(fileExt.Trim('.').ToLower(), fileExt));
            }
            return "|".Join(filters);
        }
    }
}
