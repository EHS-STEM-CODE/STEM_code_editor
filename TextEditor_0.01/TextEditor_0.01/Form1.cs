using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;  //default, but not needed unless you are drawing
using System.Linq;     //same
using System.Text;
using System.Windows.Forms;

namespace TextEditor_0._01
{
    public partial class MainForm : Form  //probably not the best name for the form. Maybe MainForm or EditingForm.
    {
        private String fileName;
        private FileEditor fileEditor;

        private bool isFirstChange = true;
        public MainForm()
        {
            fileEditor = null;
            InitializeComponent();
            OnNewFile();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if you're file is dirty, are you sure you want to exit?
            System.Environment.Exit(-1);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader openFile = new System.IO.StreamReader(openFileDialog1.FileName);
                fileEditor.SetPath(openFileDialog1.FileName);
                textBox1.Text = openFile.ReadToEnd();
                openFile.Close();
                tabControl1.SelectedTab.Text = fileEditor.TabLabel();//System.IO.Path.GetFileName(openFileDialog1.FileName);
                isFirstChange = true;  //maybe something like fileIsDirty would be better.
            }
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void newTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //adding a new tab means adding a new (empty) file to edit or opening another file.
            //this implies that you need something that represents and open file that you're editing.
            // --name, shortname, cursor position, dirty/clean, undo stack, etc.
            //switching tabs means switching edit file contexts.
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName != "untitled")
            {


            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamWriter saveFile = new System.IO.StreamWriter(saveFileDialog1.FileName);
                fileEditor.SetPath(saveFileDialog1.FileName);
                String[] lines = textBox1.Lines;
                foreach (string line in lines)
                    saveFile.WriteLine(line);
                saveFile.Close();
                tabControl1.SelectedTab.Text = fileEditor.Path(); //System.IO.Path.GetFileName(saveFileDialog1.FileName);
                fileEditor.SetDirty(false);
                fileEditor.SetNew(false);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("Make me colorful"))
                textBox1.ForeColor = Color.Sienna;
            if (textBox1.Text.Contains("Hide me"))
                textBox1.ForeColor = Color.White;
            if (textBox1.Text.Contains("Bring me back!"))
                textBox1.ForeColor = Color.Black;
            if (isFirstChange == true)
            {
                tabControl1.SelectedTab.Text = tabControl1.SelectedTab.Text + "*";
                isFirstChange = false;
            }
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OnNewFile();
        }

        private void OnNewFile()
        {
            textBox1.Text = "";
            fileEditor = new FileEditor();
            tabControl1.SelectedTab.Text = fileEditor.TabLabel();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

public class FileEditor
{
    private string path;
    private string shortName;
    bool isDirty;
    bool isNew;

    public FileEditor()
    {
        isDirty = false;
        isNew = true;
        path = "";
        shortName = "untitled";
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
}
