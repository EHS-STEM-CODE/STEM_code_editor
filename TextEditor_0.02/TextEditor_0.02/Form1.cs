using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScintillaNET;

namespace TextEditor_0._02
{
    public partial class mainWindow : Form
    {
        public mainWindow()
        {
            InitializeComponent();
            scintilla1.Margins[0].Width = 32;   //Margins allow for up to 9999 lines -> Then it wraps around
            ScintillaNET.ConfigurationManager.Language = "html";
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
            richTextBox1.Text = Highlight(richTextBox1.Text);

    */