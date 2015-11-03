using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;  //default, but not needed unless you are drawing
using System.Linq;     //same
using System.Text;
using System.Windows.Forms;
using System.IO;
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
            tabControl.Selected += TabControl_Selected;
            menuStrip1.Dock = DockStyle.Top;
            tabControl.Dock = DockStyle.Fill;
            Controls.Add(tabControl);
            Controls.Add(menuStrip1);       //Fixed the problem <- Can this line be avoided
            OnNewFile();
        }

        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            currentFileEditor = (FileEditor)fileEditors[tabControl.SelectedIndex];
        }



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnOpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!currentFileEditor.IsNew())
                save(currentFileEditor.Path());
        }

        private void save(String path)
        {
            System.IO.StreamWriter saveFile = new System.IO.StreamWriter(path);
            String lines = currentFileEditor.GetText();
            saveFile.Write(lines);
            saveFile.Close();
            currentFileEditor.SetDirty(false);
            currentFileEditor.SetNew(false);
            tabControl.SelectedTab.Text = currentFileEditor.ShortName();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        private bool saveAs()
        {
            bool success = false;
            System.Windows.Forms.DialogResult result = saveFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                save(saveFileDialog1.FileName);
                currentFileEditor.SetPath(saveFileDialog1.FileName);
                currentFileEditor.SetShortName(Path.GetFileName(saveFileDialog1.FileName));
                currentFileEditor.SetNew(false);
                tabControl.SelectedTab.Text = currentFileEditor.ShortName();
                success = true;
            }
            return success;
        }

        private void OnNewFile()
        {
            currentFileEditor = new FileEditor(tabControl);
            fileEditors.Add(currentFileEditor);
            TabPage newTab = new TabPage(currentFileEditor.ShortName());
            tabControl.TabPages.Add(newTab);

            newTab.Controls.Add(currentFileEditor.getScintilla());
            tabControl.SelectTab(fileEditors.Count - 1);
        }

        private void OnOpenFile()
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader openFile = new System.IO.StreamReader(openFileDialog1.FileName);
                FileEditor newFileEditor = new FileEditor(tabControl, openFileDialog1.FileName, openFileDialog1.SafeFileName, openFile.ReadToEnd());
                TabPage newTab = new TabPage(openFileDialog1.SafeFileName);
                tabControl.TabPages.Add(newTab);
                currentFileEditor = newFileEditor;
                fileEditors.Add(newFileEditor);
                newTab.Controls.Add(currentFileEditor.getScintilla()); 
                tabControl.SelectTab(fileEditors.Count-1);        

                openFile.Close();
            }
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
            e.Cancel = !closeFile(); //don't close if the closing of files failed.
        }

        //!!this is really appQuit().
        //!!we need a separate CloseFile() that just closes the current tab (check to save if dirty).
        private bool closeFile()
        {
            bool success = true;
            for (int i = 0; i < fileEditors.Count; i++ )
            {
                currentFileEditor = (FileEditor)fileEditors[i];
                tabControl.SelectTab(i);
                if (currentFileEditor.IsDirty())
                {
                    //need to abort closing when the user clicks cancel
                    if (currentFileEditor.IsNew())
                    {
                        DialogResult result = MessageBox.Show("Do you want to save this file?", "Saving untitled file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            success = saveAs();
                        }
                        else if (result == DialogResult.No)
                            Console.Write("Close the file and dump out of text editor"); // -> remove this from tabControl and fileEditors
                        else if (result == DialogResult.Cancel)
                            success = false;
                    }
                    else // file is not new but dirty
                    {
                        DialogResult result = MessageBox.Show("Do you want to save " + currentFileEditor.ShortName() + "?", "Saving file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                            save(currentFileEditor.Path());
                        else if (result == DialogResult.No)
                            Console.Write("close the tab, remove"); // remove from tabControl and fileEditors
                        else if (result == DialogResult.Cancel)
                            success = false;
                    }
                }
            }
            return success;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnNewFile();
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
    }
    
}
