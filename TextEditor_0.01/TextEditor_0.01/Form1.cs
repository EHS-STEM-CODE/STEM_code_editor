using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextEditor_0._01
{
    public partial class Form1 : Form
    {
        private String fileName;
        private bool isFirstChange = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(-1);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader openFile = new System.IO.StreamReader(openFileDialog1.FileName);
                fileName = openFileDialog1.FileName;
                textBox1.Text = openFile.ReadToEnd();
                openFile.Close();
                tabControl1.SelectedTab.Text = System.IO.Path.GetFileName(openFileDialog1.FileName);
                isFirstChange = true;
            }
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void newTabToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                System.IO.StreamWriter saveFile = new System.IO.StreamWriter(openFileDialog1.FileName);
                fileName = openFileDialog1.FileName;
                String[] lines = textBox1.Lines;
                foreach (string line in lines)
                    saveFile.WriteLine(line);
                saveFile.Close();
                tabControl1.SelectedTab.Text = System.IO.Path.GetFileName(openFileDialog1.FileName);
                isFirstChange = true;
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
            textBox1.Text = "";
            tabControl1.SelectedTab.Text = "untitled";
            fileName = "untitled";
        }

        
    }
}
