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

    public FileEditor()
    {
        isDirty = false;
        isNew = true;
        path = "";
        shortName = "untitled";
        sci = new Scintilla();
        initializeScintilla();

    }

    private void initializeScintilla()
    {
        sci.Margins[0].Width = 32;
        sci.Lexer = Lexer.Cpp;
        sci.Size = new Size(700, 700);  //How do I set it to make it lock to the screen size?
        sci.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
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

    public string ShortName()
    {
        return shortName;
    }

    public String getText()
    {
        return (sci.GetTextRange(0, sci.TextLength));
    }

    public void SetText(String moreText)
    {
        sci.AppendText(moreText);
    }
}
