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
            this.DoubleBuffered = true;
            OnNewWindow();
        }

        private void OnNewWindow()
        {
            currentFileEditor = null;
            fileEditors = new ArrayList();
            tabControl = new TabControl();
            tabControl.Name = "tabControl";

            tabControl.Selected += TabControl_Selected;     //TabControl_Selected callled on tab click
      

            //!! dock control fill loses the top of the tabs under the menu.
            tabControl.Location = new Point(0, 25);
            tabControl.Size = new Size(768, 765);
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            Controls.Add(tabControl);
            OnNewFile();
        }

        //Method called on tab control click
        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            Console.Write(tabControl.SelectedIndex);
            //!! set currentFileEditor to the fileEditor at index tabControl.SelectedIndex;  Don't know how to do this in C#
            //currentFileEditor = fileEditors.
        }



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnOpenFile();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentFileEditor.SetText("");
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
            if(!currentFileEditor.IsNew())save(currentFileEditor.Path());
        }

        private void save(String path)
        {
            System.IO.StreamWriter saveFile = new System.IO.StreamWriter(path);
            String lines = currentFileEditor.getText();
            saveFile.Write(lines);
            saveFile.Close();
            currentFileEditor.SetDirty(false);
            currentFileEditor.SetNew(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
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
            tabControl.SelectTab(fileEditors.Count - 1);    //Select the tab that was just created
        }

        private void OnOpenFile()
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader openFile = new System.IO.StreamReader(openFileDialog1.FileName);

                currentFileEditor = new FileEditor();       //Create a new file editor
                fileEditors.Add(currentFileEditor);         //Add it to the arrayList
                TabPage newTab = new TabPage(openFileDialog1.FileName); //Create a new tab
                tabControl.TabPages.Add(newTab);            //Add the new tab to the tabController
                
                newTab.Controls.Add(currentFileEditor.getScintilla()); //Add the new text box to the tab    
                tabControl.SelectTab(fileEditors.Count-1);        

                currentFileEditor.SetPath(openFileDialog1.FileName);
                currentFileEditor.SetText(openFile.ReadToEnd());
                openFile.Close();
            }
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
                save(saveFileDialog1.FileName);
                currentFileEditor.SetPath(saveFileDialog1.FileName);
                currentFileEditor.SetNew(false);
            }
        }

        //maybe a better way to update the display
        //!! NEEDED?
        private void UpdateDisplay()
        {
            tabControl.SelectedTab.Text = currentFileEditor.TabLabel();
        }


        //!! you many want to just remove these
        // should not have a flicker problem.
        //protected override void OnResizeBegin(EventArgs e)      //Next two methods prevent flickering while resizing the window
        //{
        //    SuspendLayout();
        //    base.OnResizeBegin(e);
        //}
        //protected override void OnResizeEnd(EventArgs e)
        //{
        //    ResumeLayout();
        //    base.OnResizeEnd(e);
        //}

        private void tabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnNewFile();
        }
    }
    
}
