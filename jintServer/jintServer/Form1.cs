using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace jintServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            outputBox.Text = "Hello Derek! \r this text box is called outputBox";
            codeBox.Text = "Here are your two textboxes \r this text box is called codeBox";
        }

    }
}
