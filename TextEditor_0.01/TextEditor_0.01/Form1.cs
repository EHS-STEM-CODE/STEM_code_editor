using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;  //default, but not needed unless you are drawing
using System.Linq;     //same
using System.Text;
using System.Windows.Forms;

namespace TextEditor_0._01
{
    public partial class MainForm : Form
    {
        private FileEditor currentFileEditor;
        private ArrayList fileEditors;
        private bool isFirstChange = true;
        public MainForm()
        {
            currentFileEditor = null;
            fileEditors = new ArrayList();
            InitializeComponent();
            OnNewFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader openFile = new System.IO.StreamReader(openFileDialog1.FileName);
                currentFileEditor = new FileEditor();
                fileEditors.Add(currentFileEditor);
                currentFileEditor.SetPath(openFileDialog1.FileName);
                textBox1.Text = openFile.ReadToEnd();
                openFile.Close();
                tabControl1.SelectedTab.Text = System.IO.Path.GetFileName(openFileDialog1.FileName);
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
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamWriter saveFile = new System.IO.StreamWriter(saveFileDialog1.FileName);
                currentFileEditor.SetPath(saveFileDialog1.FileName);
                String[] lines = textBox1.Lines;
                foreach (string line in lines)
                    saveFile.WriteLine(line);
                saveFile.Close();
                tabControl1.SelectedTab.Text = System.IO.Path.GetFileName(saveFileDialog1.FileName);
                currentFileEditor.SetDirty(false);
                currentFileEditor.SetNew(false);
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
            if (currentFileEditor.IsDirty() == false)
            {
                tabControl1.SelectedTab.Text = tabControl1.SelectedTab.Text + "*";
                currentFileEditor.SetDirty(true);
            }
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OnNewFile();
        }

        private void OnNewFile()
        {
            textBox1.Text = "";
            currentFileEditor = new FileEditor();
            tabControl1.SelectedTab.Text = currentFileEditor.TabLabel();
            fileEditors.Add(currentFileEditor);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            saveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private void saveAs()
        {
            foreach (FileEditor fileEditor in fileEditors)
            {
                if (fileEditor.IsDirty())
                {
                    DialogResult result = MessageBox.Show("Do you want to save \"" + fileEditor.ShortName() + "\"", "Important Query", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            System.IO.StreamWriter saveFile = new System.IO.StreamWriter(saveFileDialog1.FileName);
                            currentFileEditor.SetPath(saveFileDialog1.FileName);
                            String[] lines = textBox1.Lines;
                            foreach (string line in lines)
                                saveFile.WriteLine(line);
                            saveFile.Close();
                            tabControl1.SelectedTab.Text = currentFileEditor.Path(); //System.IO.Path.GetFileName(saveFileDialog1.FileName);
                            currentFileEditor.SetDirty(false);
                            currentFileEditor.SetNew(false);
                            System.Environment.Exit(-1);
                        }
                    }
                    else if (result == DialogResult.No)
                    {
                        System.Environment.Exit(-1);
                    }
                }
            }
        }
    }
}
