﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScintillaNET;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

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
        InitializeScintilla();
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
        InitializeScintilla();
    }

    private void InitializeScintilla()
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
        sci.MarginClick += Scintilla_MarginClick;

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

        sci.HScrollBar = false;

        // Enable automatic folding
        sci.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
        
        //Configure margins to display bookmarks
        sci.Margins[1].Width = 16;
        sci.Margins[1].Sensitive = true;
        sci.Margins[1].Type = MarginType.Symbol; 
        //sci.Margins[3].Mask = Marker.MaskAll; //This line messes with the line folding
        sci.Margins[1].Cursor = MarginCursor.Arrow;

        sci.Markers[1].Symbol = MarkerSymbol.Circle;
        sci.Markers[1].SetBackColor(Color.DeepSkyBlue);
        sci.Markers[1].SetForeColor(Color.Black);
    }

    public Scintilla GetScintilla()
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

    private void Scintilla_MarginClick(object sender, MarginClickEventArgs e)
    {
        if (e.Margin == 1)
        {
            // Do we have a marker for this line?
            const uint mask = (1 << 1);
            var line = sci.Lines[sci.LineFromPosition(e.Position)];
            if ((line.MarkerGet() & mask) > 0)
            {
                // Remove existing bookmark
                line.MarkerDelete(1);
            }
            else
            {
                // Add bookmark
                line.MarkerAdd(1);
            }
        }
    }
   
    public ArrayList GetBreakpoints()           //Break point functionallity is not used currently due to the lack of debugger options, ready for future implementation.
    {
        ArrayList bPoints = new ArrayList();
        const uint mask = (1 << 1);
        for (int i = 0; i < sci.Lines.Count; i++)
        {
            if ((sci.Lines[i].MarkerGet() & mask) > 0) bPoints.Add((i+1));
        }
        return (bPoints);
    }
}
