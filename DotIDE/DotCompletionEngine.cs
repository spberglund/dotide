using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Drawing.Text;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Xml;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;


namespace DotIDE
{
    public class DotCompletionEngine
    {
        public string dotFile;
        public string graphFile;

        public bool autoCompileEnabled = false;

        private string _dotSaveFile = "";
        public string dotSaveFile {
            get
            {
                return _dotSaveFile;
            }
            set
            {
                _dotSaveFile = value;
                graphSaveFile = !string.IsNullOrWhiteSpace(value) ?
                    Path.Combine(Path.GetDirectoryName(value), Path.GetFileNameWithoutExtension(value)) + "." + Properties.Settings.Default.outFmt
                    : "";
            }
        } //Path to Load and save from
        public string graphSaveFile { get; private set; }    //Path to Save final output to

        public MainForm parent;
        
        public string[] textFileExts = { "gv", "dot", "plain" };
        public string[] imgFileExts = { "gif", "jpg", "jpeg", "png" };

        private TextEditor dotEditor;

        private Regex validId = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]+|-?[0-9]*\.[0-9]*|<.+>|"".*[^\\]""", RegexOptions.Compiled);

        private CompletionWindow dotCompWindow;

        private DotCompleteCommand completeCommand;

        private Dictionary<string, List<string>> edgeAttrs;
        private Dictionary<string, List<string>> nodeAttrs;
        private Dictionary<string, List<string>> graphAttrs;
        private Dictionary<string, List<string>> clusterAttrs;

        private HashSet<string> nodeIDs;

        private bool inQuotes = false;
        private bool inDict = false;
        private bool escaped = false;

        private string curLine = "";
        private char prevChar;
        private DotContext curContext = DotContext.NodeID;

        public DotCompletionEngine(TextEditor editor, MainForm form)
        {
            parent = form;
            dotEditor = editor;

            nodeIDs = new HashSet<string>(){
                "graph",
                "digraph",
                "node",
                "subgraph",
                "edge",
            };

            dotSaveFile = "";

            LoadEngine();

            RefreshFileNames();

            ApplySettings();

            completeCommand = new DotCompleteCommand();
            completeCommand.ExecuteCalled += Complete;
            
            dotEditor.TextArea.DefaultInputHandler.Detach();
            dotEditor.TextArea.DefaultInputHandler.Editing.AddBinding(completeCommand, ModifierKeys.Control, Key.Space, null);
            dotEditor.TextArea.DefaultInputHandler.Attach();

            dotEditor.TextArea.TextEntering += TextArea_TextEntering;
            dotEditor.TextArea.TextEntered += TextArea_TextEntered;

            dotEditor.TextArea.AllowDrop = true;
            dotEditor.TextArea.DragEnter += TextArea_DragEnter;
            dotEditor.TextArea.Drop += TextArea_Drop;
        }

        void TextArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link;
        }

        void TextArea_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                UpdateContext();
                if (curContext == DotContext.NodeID)
                {
                    dotEditor.TextArea.PerformTextInput(
                        Idize(Path.GetFileNameWithoutExtension(files[0])) + " [image=\"" + files[0] + "\" label=\"\"]\n");
                }
                else if (curContext == DotContext.NodeAttribute || curContext == DotContext.EdgeAttribute)
                {
                    dotEditor.TextArea.PerformTextInput(
                        "image=\"" + files[0] + "\"");
                }
                else if (curContext == DotContext.NodeValue || curContext == DotContext.EdgeValue)
                    dotEditor.TextArea.PerformTextInput(
                        "\"" + files[0] + "\"");
            }
            else
            {
                if (autoCompileEnabled) CompileDot();
            }
        }

        //Doesn't actually work
        public string Idize(string id)
        {
            return validId.IsMatch(id) ? id : "\"" + id + "\"";
        }

        void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (autoCompileEnabled) CompileDot();
        }
        public void Save(string file="")
        {
            if (!string.IsNullOrWhiteSpace(file))
                dotSaveFile = file;
            try
            {
                dotEditor.Save(dotSaveFile);
                File.Copy(graphFile, graphSaveFile, true);
            }
            catch { }
        }

        public void Open(string file)
        {
            dotSaveFile = file;
            try { dotEditor.Text = File.ReadAllText(file); }
            catch { }
            if(autoCompileEnabled) CompileDot();
        }
        public void RefreshFileNames()
        {
            dotFile = Path.GetTempFileName() + ".gv";
            graphFile = Path.GetTempFileName() + "." + Properties.Settings.Default.outFmt;
            if (textFileExts.Contains(Properties.Settings.Default.outFmt))
            {
                graphFile += ".xdot";
            }
            File.Delete(dotFile);
            File.Delete(graphFile);
        }
        public void RefreshNodes()
        {
            nodeIDs.Clear();
            bool inDi = false; 
            foreach (char c in dotEditor.Text)
            {

            }
        }
        /// <summary>
        /// Produce autocomplete dropdown if relevent
        /// </summary>
        private void Complete(object sender=null, EventArgs e=null)
        {
            UpdateContext();
            if (curContext == DotContext.NodeID)
                RefreshNodes();
            Console.WriteLine(curContext);
            BuildCompletionWindow();
        }
        /// <summary>
        /// Forces an update of the current context
        /// </summary>
        public void UpdateContext()
        {
            curContext = DotContext.NodeID;
            curLine = dotEditor.CaretLine();
            if (curLine == null) { return; }
            inQuotes = false;
            escaped = false;
            inDict = false;
            foreach (char c in curLine.Take(dotEditor.TextArea.Caret.Column-1))
            {
                PushNewChar(c);
            }
        }
        private void BuildCompletionWindow()
        {
            string wordStart = GetContextWord();
            var relWords = GetContextSugestions(wordStart);
            if (relWords.Count() == 0) { return; }
            dotCompWindow = new CompletionWindow(dotEditor.TextArea);
            dotCompWindow.WindowStyle = WindowStyle.None;
            dotCompWindow.ResizeMode = ResizeMode.NoResize;
            dotCompWindow.ClipToBounds = false;
            dotCompWindow.CloseWhenCaretAtBeginning = true;
            dotCompWindow.CompletionList.UseLayoutRounding = true;

            var data = dotCompWindow.CompletionList.CompletionData;
            foreach(var item in relWords)
                data.Add(new DotCompletionData(item.Item1, wordStart.Length, 1, item.Item2));
            if (data.Count > 0)
                dotCompWindow.CompletionList.ListBox.SelectedIndex = 0;
            dotCompWindow.Closed += delegate { dotCompWindow = null; };
            dotCompWindow.Show();
        }
        private string GetContextWord()
        {
            switch (curContext)
            {
                case DotContext.NodeID:
                case DotContext.EdgeAttribute:
                case DotContext.NodeAttribute:
                case DotContext.GraphAttribute:
                case DotContext.ClusterAttribute:
                    return new string(curLine.Take(dotEditor.TextArea.Caret.Column - 1).
                        Reverse().TakeWhile(c => char.IsLetterOrDigit(c)).Reverse().ToArray());
                case DotContext.EdgeValue:
                case DotContext.NodeValue:
                case DotContext.GraphValue:
                case DotContext.ClusterValue:
                    return new string(curLine.Take(dotEditor.TextArea.Caret.Column - 1).
                        Reverse().TakeWhile(c => c != '=').Reverse().ToArray());
                default:
                    return "";
            }
        }
        private IEnumerable<Tuple<string, ImageSource>> GetContextSugestions(string beg="")
        {
            IEnumerable<string> words = Enumerable.Empty<string>();
            switch (curContext)
            {
                case DotContext.NodeID: words = nodeIDs; break;
                case DotContext.EdgeAttribute: words = edgeAttrs.Keys; break;
                case DotContext.NodeAttribute: words = nodeAttrs.Keys; break;
                case DotContext.GraphAttribute: words = graphAttrs.Keys; break;
                case DotContext.ClusterAttribute: words = clusterAttrs.Keys; break;
                case DotContext.EdgeValue:
                case DotContext.NodeValue:
                case DotContext.GraphValue:
                case DotContext.ClusterValue:
                    string key = new string(curLine.Take(dotEditor.TextArea.Caret.Column - 1).Reverse().
                        SkipWhile(c => c != '=').SkipWhile(c => "= ".Contains(c)).
                        TakeWhile(c => !"[;, ".Contains(c)).Reverse().ToArray());

                    if (key == "image")
                    {
                        return GetLocalImages();
                    }
                    Dictionary<string, List<string>> attrDict = null;
                    switch (curContext)
	                {
                        case DotContext.EdgeValue: attrDict = edgeAttrs; break;
                        case DotContext.NodeValue: attrDict = nodeAttrs; break;
                        case DotContext.GraphValue: attrDict = graphAttrs; break;
                        case DotContext.ClusterValue: attrDict = clusterAttrs; break;
	                }
                    if (attrDict != null && attrDict.ContainsKey(key)) { words = attrDict[key]; }
                    break;
                default: break;
            }
            return from w in words where w.StartsWith(beg) select Tuple.Create<string, ImageSource>(w, null);
        }
        private IEnumerable<Tuple<string, ImageSource>> GetLocalImages()
        {
            var imgSrcConv = new ImageSourceConverter();
            string projDir = File.Exists(dotSaveFile) ? Path.GetDirectoryName(dotSaveFile) : "";
            string picDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            var locImgs = Directory.Exists(picDir) ?
                from f in Directory.EnumerateFiles(picDir, "*.*", SearchOption.AllDirectories)
                where imgFileExts.Contains(Path.GetExtension(f).ToLower().Trim('.'))
                select Tuple.Create('"' + f + '"', (ImageSource)imgSrcConv.ConvertFromString(f))
                : Enumerable.Empty<Tuple<string, ImageSource>>();

            var projImgs = Directory.Exists(projDir) ?
                from f in Directory.EnumerateFiles(projDir, "*.*", SearchOption.AllDirectories)
                where imgFileExts.Contains(Path.GetExtension(f).ToLower().Trim('.'))
                select Tuple.Create('"' + f + '"', (ImageSource)imgSrcConv.ConvertFromString(f))
                : Enumerable.Empty<Tuple<string, ImageSource>>();

            return Enumerable.Concat(projImgs, locImgs);
        }
        private void ApplySettings()
        {
            dotEditor.FontSize = 14.0;
            dotEditor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            dotEditor.FontWeight = System.Windows.FontWeights.Bold;

            dotEditor.ShowLineNumbers = true;

            var opts = new TextEditorOptions();
            opts.HighlightCurrentLine = true;
            opts.ConvertTabsToSpaces = false;
            //opts.ShowColumnRuler = true;
            opts.ShowBoxForControlCharacters = true;
            dotEditor.Options = opts;

            using (Stream s = Utils.GetFileStream("dot.xshd"))
            {
                if (s != null)
                {
                    using (XmlTextReader reader = new XmlTextReader(s))
                    {
                        dotEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    }
                }
            }
        }
        public void CompileDot(string outFile="")
        {
            if (outFile == "") outFile = graphFile;
            //File.WriteAllText(dotFile, dotEditor.Text);
            var dotProc = new Process();

            dotProc.StartInfo.UseShellExecute = false;
            dotProc.StartInfo.RedirectStandardOutput = true;
            dotProc.StartInfo.RedirectStandardError = true;
            dotProc.StartInfo.RedirectStandardInput = true;
            dotProc.StartInfo.CreateNoWindow = true;
            dotProc.StartInfo.FileName = Properties.Settings.Default.dotPath;
            dotProc.StartInfo.Arguments = string.Format("-T{0} -o \"{1}\"", Properties.Settings.Default.outFmt, outFile);
            dotProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            dotProc.EnableRaisingEvents = true;
            dotProc.Exited += parent.CompileFinished;
            try
            {
                dotProc.Start();
                dotProc.StandardInput.AutoFlush = true;
                dotProc.StandardInput.WriteLine(dotEditor.Text);
                dotProc.StandardInput.Close();
            }
            catch { }
        }
        public void LoadEngine()
        {
            using(Stream s = Utils.GetFileStream("dotComp.json"))
            {
                var dotCont = JsonConvert.DeserializeObject<JsonDotCompletionContext>((new StreamReader(s)).ReadToEnd());

                if(dotCont.Types.ContainsKey("font"))
                {
                    var fonts = new InstalledFontCollection();
                    dotCont.Types["font"].AddRange(from f in fonts.Families select '"' + f.Name + '"');
                }
                
                edgeAttrs = (from a in dotCont.Attributes where a.Value.UsedBy.Contains('E') select a).ToDictionary(p => p.Key, p => dotCont.Types[p.Value.Type]);
                nodeAttrs = (from a in dotCont.Attributes where a.Value.UsedBy.Contains('N') select a).ToDictionary(p => p.Key, p => dotCont.Types[p.Value.Type]);
                graphAttrs = (from a in dotCont.Attributes where a.Value.UsedBy.Contains('G') select a).ToDictionary(p => p.Key, p => dotCont.Types[p.Value.Type]);
                clusterAttrs = (from a in dotCont.Attributes where a.Value.UsedBy.Contains('N') select a).ToDictionary(p => p.Key, p => dotCont.Types[p.Value.Type]);

            }
            Console.WriteLine("Loaded Completion Data");
        }
        private void PushNewChar(char inChar)
        {
            if (inChar == '"' && !escaped) inQuotes = !inQuotes;
            else if (inChar == '\\' || escaped) escaped = !escaped;

            if (!inQuotes) //the context cannot change if we are in quotes
            {
                DotContext newContext = curContext;

                if(!inDict && inChar == '[') inDict = true;
                
                if (curContext == DotContext.MultiLineComment)
                {
                    if (inChar == '*' && prevChar == '/')
                        newContext = DotContext.NodeID;
                }
                else if (curContext == DotContext.Comment)
                {
                    if (inChar == '\n')
                        newContext = DotContext.NodeID;
                }
                else if (inChar == '/' && prevChar == '*')
                    newContext = DotContext.MultiLineComment;
                else if (inChar == '#' || (inChar == '/' && prevChar == '/'))
                    newContext = DotContext.Comment;
                else if (inDict)
                {
                    if (inChar == ']')
                    {
                        newContext = DotContext.NodeID;
                        inDict = false;
                    }
                    else if ("[,; \t\n".Contains(inChar))
                        switch (curContext)
                        {
                            case DotContext.EdgeAttribute:
                            case DotContext.NodeAttribute:
                            case DotContext.GraphAttribute:
                            case DotContext.ClusterAttribute:
                                break;
                            case DotContext.EdgeValue: newContext = DotContext.EdgeAttribute;
                                break;
                            case DotContext.NodeValue: newContext = DotContext.NodeAttribute;
                                break;
                            case DotContext.GraphValue: newContext = DotContext.GraphAttribute;
                                break;
                            case DotContext.ClusterValue: newContext = DotContext.ClusterAttribute;
                                break;
                            default:
                                string startLine = curLine.GetDotBase().Trim();
                                if (string.IsNullOrWhiteSpace(startLine)) { newContext = DotContext.NodeID; }
                                else if (startLine == "edge") { newContext = DotContext.EdgeAttribute; }
                                else if (startLine == "node") { newContext = DotContext.NodeAttribute; }
                                else if (startLine == "graph") { newContext = DotContext.GraphAttribute; }
                                else if (startLine.Contains("--") || startLine.Contains("->")) { newContext = DotContext.EdgeAttribute; }
                                else { newContext = DotContext.NodeAttribute; }
                                break;
                        }
                    else if (inChar == '=')
                        switch (curContext)
                        {
                            case DotContext.EdgeAttribute: newContext = DotContext.EdgeValue;
                                break;
                            case DotContext.NodeAttribute: newContext = DotContext.NodeValue;
                                break;
                            case DotContext.GraphAttribute: newContext = DotContext.GraphValue;
                                break;
                            case DotContext.ClusterAttribute: newContext = DotContext.ClusterValue;
                                break;
                            default:
                                break;
                        }
                }
                else //Not in dict
                {
                    if (prevChar == '-' && (inChar == '-' || inChar == '>'))
                        newContext = DotContext.NodeID;
                }
                Console.WriteLine("q:{0}\te:{1}\tc:{2}\td:{3}\tnc:{4}", inQuotes, escaped, inChar, inDict, newContext);
                curContext = newContext;
            }
            prevChar = inChar;
            curLine = curLine.Insert(dotEditor.TextArea.Caret.Column - 1, inChar.ToString());
        }

        /// <summary>
        /// Updates the current context
        /// </summary>
        private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && dotCompWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    dotCompWindow.CompletionList.RequestInsertion(e);
                }
            }
        }
    }
    public class DotCompleteCommand : ICommand
    {
        public event EventHandler ExecuteCalled;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            if(ExecuteCalled != null)
            {
                ExecuteCalled(this, new EventArgs());
            }
        }
    }
    /// <summary>
    /// What context should the auto-completion be considered in?
    /// </summary>
    public enum DotContext
    {
        NodeID,
        EdgeAttribute,
        NodeAttribute,
        GraphAttribute,
        ClusterAttribute,
        EdgeValue,
        NodeValue,
        GraphValue,
        ClusterValue,
        Comment,
        MultiLineComment,
    }
    public class JsonDotCompletionContext
    {
        [JsonProperty("attributes")]
        public Dictionary<string, DotAttribute> Attributes { get; set; }
        [JsonProperty("types")]
        public Dictionary<string, List<string>> Types { get; set; }

    }
    public class DotAttribute
    {
        [JsonProperty("usedby")]
        public string UsedBy { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
