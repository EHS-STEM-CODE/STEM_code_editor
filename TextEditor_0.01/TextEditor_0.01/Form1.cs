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
            tabControl.Selected += TabControl_Selected;
            menuStrip1.Dock = DockStyle.Top;
            tabControl.Dock = DockStyle.Fill;
            Controls.Add(tabControl);
            Controls.Add(menuStrip1);
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

        private void newTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //adding a new tab means adding a new (empty) file to edit or opening another file.
            //this implies that you need something that represents and open file that you're editing.
            // --name, shortname, cursor position, dirty/clean, undo stack, etc.
            //switching tabs means switching edit file contexts.
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

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
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
