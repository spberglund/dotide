using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using System.Windows.Media;

namespace DotIDE
{
    public class DotCompletionData : ICompletionData
    {
        public int InitLen = 0;
        public DotCompletionData(string text = "", int initLen = 0, double priority = 1, ImageSource image = null)
        {
            InitLen = initLen;
            Text = text;
            Priority = priority;
            Image = image;
        }
        public void Complete(TextArea textArea, ISegment compSeg, EventArgs e)
        {
            textArea.Document.Replace(compSeg, this.Text.Substring(InitLen));
            textArea.PerformTextInput("");
        }

        public object Content { get { return Text; } }

        public object Description
        {
            get { return ""; }
        }

        public ImageSource Image { get; set; }
        public double Priority { get; set; }
        public string Text { get; set; }
    }
}
