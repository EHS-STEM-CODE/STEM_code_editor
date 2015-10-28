using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;  //default, but not needed unless you are drawing
using System.Linq;     //same
using System.Text;
using System.Windows.Forms;
using ScintillaNET;

namespace TextEditor_0._01
{
    public partial class MainForm : Form
    {
        private FileEditor currentFileEditor;
        private ArrayList fileEditors;
        private TabControl tabControl;
        public MainForm()
        {
            InitializeComponent();
            OnNewWindow();
        }

        private void OnNewWindow()
        {
            currentFileEditor = null;
            fileEditors = new ArrayList();
            tabControl = new TabControl();
            tabControl.Name = "tabControl";
            tabControl.Location = new Point(0, 25);
            tabControl.Size = new Size(768, 765);
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            Controls.Add(tabControl);
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
                currentFileEditor.setText(openFile.ReadToEnd());
                openFile.Close();
                tabControl.SelectedTab.Text = System.IO.Path.GetFileName(openFileDialog1.FileName);
            }
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentFileEditor.setText("");
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
                String lines = currentFileEditor.getText();
                saveFile.Write(lines);
                //String[] lines = currentFileEditor.getText();
                //foreach (string line in lines)
                //    saveFile.WriteLine(line);
                saveFile.Close();
                tabControl.SelectedTab.Text = System.IO.Path.GetFileName(saveFileDialog1.FileName);
                currentFileEditor.SetDirty(false);
                currentFileEditor.SetNew(false);
            }
        }

      

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void OnNewFile()
        {
            currentFileEditor = new FileEditor();       //Create a new file editor
            fileEditors.Add(currentFileEditor);         //Add it to the arrayList
            TabPage newTab = new TabPage(currentFileEditor.ShortName()); //Create a new tab
            tabControl.TabPages.Add(newTab);                            //Add the new tab to the tabController

            newTab.Controls.Add(currentFileEditor.getScintilla()); //Add the new text box to the tab            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            closeFile();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //closeFile();
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Console.WriteLine("The application is closing...");
            DialogResult result = MessageBox.Show("Do you really want to quit? ", "Closing Application", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            e.Cancel = !(result == DialogResult.Yes);
        }


        private void closeFile()
        {
            foreach (FileEditor fileEditor in fileEditors)
            {
                if (fileEditor.IsDirty())
                {
                    DialogResult result = MessageBox.Show("Do you want to save \"" + fileEditor.ShortName() + "\"", "Important Query", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        saveAs();
                    }
                    else if (result == DialogResult.No)
                    {
                        System.Environment.Exit(-1);
                    }
                }
            }
        }
        private void saveAs()
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamWriter saveFile = new System.IO.StreamWriter(saveFileDialog1.FileName);
                currentFileEditor.SetPath(saveFileDialog1.FileName);
                String lines = currentFileEditor.getText();
                saveFile.Write(lines);

                //String[] lines = currentFileEditor.getText();
                //foreach (string line in lines)
                //    saveFile.WriteLine(line);
                saveFile.Close();
                tabControl.SelectedTab.Text = currentFileEditor.Path(); //System.IO.Path.GetFileName(saveFileDialog1.FileName);
                currentFileEditor.SetDirty(false);
                currentFileEditor.SetNew(false);
                System.Environment.Exit(-1);
            }
        }

        //maybe a better way to update the display
        private void UpdateDisplay()
        {
            tabControl.SelectedTab.Text = currentFileEditor.TabLabel();
        }

        protected override void OnResizeBegin(EventArgs e)      //Next two methods prevent flickering while resizing the window
        {
            SuspendLayout();
            base.OnResizeBegin(e);
        }
        protected override void OnResizeEnd(EventArgs e)
        {
            ResumeLayout();
            base.OnResizeEnd(e);
        }

        private void tabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnNewFile();
        }
    }
    
}
