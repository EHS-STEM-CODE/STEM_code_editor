using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScintillaNET;
using System.Windows.Forms;
using System.Drawing;

public class FileEditor
{
    private Scintilla sci;
    private string path;
    private string shortName;
    
    private bool isDirty;
    private bool isNew;
    private TabControl tabControl;

    public FileEditor(TabControl tabControl)
    {
        isDirty = false;
        isNew = true;
        path = "";
        shortName = "untitled";
        this.tabControl = tabControl;
        sci = new Scintilla();
        initializeScintilla();
    }

    public FileEditor(TabControl _tabControl, string _path, string _shortName, string _text)
    {
        isDirty = false;
        isNew = false;
        tabControl = _tabControl;
        path = _path;
        shortName = _shortName;
        sci = new Scintilla();
        sci.Text = _text;
        initializeScintilla();
    }

    private void initializeScintilla()
    {
        sci.Margins[0].Width = 32;
        sci.Lexer = Lexer.Cpp;
        sci.Dock = DockStyle.Fill;
        Console.WriteLine(sci.DescribeKeywordSets());

        sci.Styles[Style.Default].Font = "Consolas";
        sci.Styles[Style.Default].Size = 12;

        // Configure the CPP (C#, javascript) lexer styles
        sci.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
        sci.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
        sci.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
        sci.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
        sci.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
        sci.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
        sci.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
        sci.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
        sci.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
        sci.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
        sci.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
        sci.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
        sci.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
        sci.Styles[Style.Cpp.GlobalClass].ForeColor = Color.Coral;
        sci.Styles[Style.Cpp.CommentDocKeyword].ForeColor = Color.LightBlue;

        sci.Styles[Style.BraceLight].BackColor = Color.LightGray;
        sci.Styles[Style.BraceLight].ForeColor = Color.BlueViolet;
        sci.Styles[Style.BraceBad].ForeColor = Color.Red;
        //javascript keywords:  Style: Word
        sci.SetKeywords(0, @"abstract arguments break case catch const continue debugger default delete do
                                        else eval export extends false final finally for function goto if implements
                                        import in instanceof interface let native new null package private protected 
                                        public return short static super switch synchronized this throw throws 
                                        transient true try typeof var void volatile while with yield");

        //javascript secondary keywords (types)  - Style: Word2
        sci.SetKeywords(1, @"boolean byte char class double enum float int long");

        //documentation keywords (for things like doxygen)
        sci.SetKeywords(2, @"author authors brief");

        //javascript global classes and typedef keywords Style: GlobalClass
        sci.SetKeywords(3, @"Array Date eval hasOwnProperty Infinity isFinite isNaN isPrototypeOf length
                                        Math NaN name Number Object prototype String toString undefined valueOf");

        sci.TextChanged += TextWasChanged;



        sci.SetProperty("fold", "1");
        sci.SetProperty("fold.compact", "1");

        // Configure a margin to display folding symbols
        sci.Margins[2].Type = MarginType.Symbol;
        sci.Margins[2].Mask = Marker.MaskFolders;
        sci.Margins[2].Sensitive = true;
        sci.Margins[2].Width = 20;

        // Set colors for all folding markers
        for (int i = 25; i <= 31; i++)
        {
            sci.Markers[i].SetForeColor(SystemColors.ControlLightLight);
            sci.Markers[i].SetBackColor(SystemColors.ControlDark);
        }

        // Configure folding markers with respective symbols
        sci.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
        sci.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
        sci.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
        sci.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
        sci.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
        sci.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
        sci.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;


        sci.AssignCmdKey(Keys.Control | Keys.N, Command.LineDown);
        sci.AssignCmdKey(Keys.Control | Keys.P, Command.LineUp);
        sci.AssignCmdKey(Keys.Control | Keys.B, Command.CharLeft);
        sci.AssignCmdKey(Keys.Control | Keys.F, Command.CharRight);
        sci.AssignCmdKey(Keys.Control | Keys.K, Command.LineCopy);
        sci.AssignCmdKey(Keys.Control | Keys.A, Command.Home);
        sci.AssignCmdKey(Keys.Control | Keys.E, Command.LineEnd);

        sci.HScrollBar = false;

        // Enable automatic folding
        sci.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
    }

    public Scintilla getScintilla()
    {
        return (sci);
    }

    public string TabLabel()
    {
        if (isDirty)
            return shortName + "*";
        else
            return shortName;
    }

    public bool IsDirty()
    {
        return isDirty;
    }

    public bool IsNew()
    {
        return isNew;
    }

    public void SetDirty(bool b)
    {
        isDirty = b;
    }

    public void SetNew(bool b)
    {
        isNew = b;
    }

    public void SetPath(string _path)
    {
        path = _path;
    }

    public string Path()
    {
        return path;
    }

    public void SetShortName(string _shortName)
    {
        shortName = _shortName;
    }

    public string ShortName()
    {
        return shortName;
    }

    public String GetText()
    {
        return (sci.GetTextRange(0, sci.TextLength));
    }

    public void TextWasChanged(object sender, EventArgs e)
    {
        isDirty = true;
        tabControl.SelectedTab.Text = TabLabel();
    }

}
