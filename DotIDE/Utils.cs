using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;

namespace DotIDE
{
    public static class Utils
    {
        public static dynamic MinVal(object obj)
        {
            return obj.GetType().InvokeMember("MinValue", BindingFlags.GetField, null, obj, null);
        }
        public static dynamic MaxVal(object obj)
        {
            return obj.GetType().InvokeMember("MaxValue", BindingFlags.GetField, null, obj, null);
        }
        public static Stream GetFileStream(string file)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(Properties.Resources).Namespace + "." + file);
        }
        public static string CaretLine(this TextEditor te)
        {
            var loc = te.TextArea.Caret.Location;
            if (loc == null) { return ""; }
            return te.Text.Split('\n')[loc.Line - 1];
        }
        public static void AppendDotText(this TextBox tb, string text)
        {
            if(text.StartsWith("Error:"))
            {
                text = new string(text.Skip(6).SkipWhile(c => c != ':').Skip(1).SkipWhile(c => c != ':').Skip(1).ToArray());
            }
            tb.AppendText(text);
        }
        public static string GetDotBase(this IEnumerable<char> str)
        {
            var sb = new StringBuilder();
            bool inQuotes = false;
            bool escaped = false;
            foreach (char c in str)
            {
                if (c == '"' && !escaped) { inQuotes = !inQuotes; }
                else if (c == '\\' || escaped) { escaped = !escaped; }
                else if (!inQuotes)
                {
                    if (c == '[') { break; }
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public static string Fmt(this string fmtStr, params object[] args)
        {
            return string.Format(fmtStr, args);
        }
        public static string Join<T>(this string joinStr, IEnumerable<T> things)
        {
            return string.Join<T>(joinStr, things);
        }
    }
}
