using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScintillaNET;

/*
 * this had some interesting application examples:
 * https://github.com/jacobslusser/ScintillaNET/wiki
 * 
 * http://vipinkumaryadav.com/
 */

namespace TextEditor_0._02
{
    public partial class mainWindow : Form
    {
        public mainWindow()
        {
            InitializeComponent();
            scintilla.Margins[0].Width = 32;   //Margins allow for up to 9999 lines -> Then it wraps around
            scintilla.Lexer = Lexer.Cpp;
            Console.WriteLine(scintilla.DescribeKeywordSets());
           

            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 12;

            // Configure the CPP (C#, javascript) lexer styles
            scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            scintilla.Styles[Style.Cpp.GlobalClass].ForeColor = Color.Coral;
            scintilla.Styles[Style.Cpp.CommentDocKeyword].ForeColor = Color.LightBlue;

            scintilla.Styles[Style.BraceLight].BackColor = Color.LightGray;
            scintilla.Styles[Style.BraceLight].ForeColor = Color.BlueViolet;
            scintilla.Styles[Style.BraceBad].ForeColor = Color.Red;

            //javascript keywords:  Style: Word
            scintilla.SetKeywords(0, @"abstract arguments break case catch const continue debugger default delete do
                                        else eval export extends false final finally for function goto if implements
                                        import in instanceof interface let native new null package private protected 
                                        public return short static super switch synchronized this throw throws 
                                        transient true try typeof var void volatile while with yield");

            //javascript secondary keywords (types)  - Style: Word2
            scintilla.SetKeywords(1,  @"boolean byte char class double enum float int long");

            //documentation keywords (for things like doxygen)
            scintilla.SetKeywords(2, @"author authors brief");

            //javascript global classes and typedef keywords Style: GlobalClass
            scintilla.SetKeywords(3, @"Array Date eval hasOwnProperty Infinity isFinite isNaN isPrototypeOf length
                                        Math NaN name Number Object prototype String toString undefined valueOf");
            //C# keywords
            /*
            scintilla1.SetKeywords(0, @"abstract as base break case catch checked continue  
                                      default delegate do else event explicit extern false 
                                      finally fixed for foreach goto if implicit in interface 
                                      internal is lock namespace new null object operator 
                                      out override params private protected public readonly 
                                      ref return sealed sizeof stackalloc switch this throw 
                                      true try typeof unchecked unsafe using virtual while");

            scintilla1.SetKeywords(1, @"bool byte char class const decimal double enum 
                                      float int long sbyte short static string struct 
                                      uint ulong ushort void");
             */
            
        }
       
      

        private void scintilla1_Click(object sender, EventArgs e)
        {

        }
    }
}


/*

            //Which is more proper
            String rawText = richTextBox1.Text;
            String rich = Highlight(rawText);
            richTextBox1.Text = rich;
            //Or
            richTextBox1.Text = Highlight(richTextBox1.Text);  // this is better. more consise. Easier to understand.

    */