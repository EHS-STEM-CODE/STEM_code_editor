using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ScintillaNET;
using Jint;

namespace TextEditor_0._01
{


    public interface ClientMessageDisplay
    {
        void displayIncomingText(string msg);
        void displayStatusText(string msg);
        void displayStatusText(string msg, string type);
    }


    public partial class MainForm : Form, ClientMessageDisplay
    {
        private FileEditor currentFileEditor;
        private ArrayList fileEditors;
        private TabControl tabControl;
        private ContextMenu mnu;
        MenuItem mnuClose;
        private static string address = "127.0.0.1";
        private static int port;
        private Client client;
        private ArrayList breakPoints;

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
            tabControl.MouseUp += tabControl_MouseUp;
            menuStrip1.Dock = DockStyle.Top;
            tabControl.Dock = DockStyle.Fill;
            splitContainer1.Panel1.Controls.Add(tabControl);
            splitContainer1.Panel1.Controls.Add(menuStrip1);       //Fixed the problem <- Can this line be avoided
            splitContainer2.Panel1.Controls.Add(tabControl);
            splitContainer2.Panel1.Controls.Add(menuStrip1);

            mnu = new ContextMenu();
            mnuClose = new MenuItem("Close");   
            mnuClose.Click += new EventHandler(mnuClose_Click);
            mnu.MenuItems.AddRange(new MenuItem[] { mnuClose });

            uploadButton.Enabled = false;
            stepButton.Enabled = false;
            breakPoints = new ArrayList();

            OnNewFile();
        }

        private void OnNewFile()
        {
            currentFileEditor = new FileEditor(tabControl);
            fileEditors.Add(currentFileEditor);
            TabPage newTab = new TabPage(currentFileEditor.ShortName());
            tabControl.TabPages.Add(newTab);

            newTab.Controls.Add(currentFileEditor.getScintilla());
            tabControl.SelectTab(fileEditors.Count - 1);

            port = 3002;
            Client client = new Client(address, port, this);
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
            if (currentFileEditor.IsDirty())
            {
                if (currentFileEditor.IsNew())
                    saveAs();
                else
                    save(currentFileEditor.Path());
            }
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

        private void OnOpenFile()
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader openFile = new System.IO.StreamReader(openFileDialog1.FileName);
                FileEditor newFileEditor = new FileEditor(tabControl, openFileDialog1.FileName, openFileDialog1.SafeFileName, openFile.ReadToEnd());
                if (currentFileEditor.IsNew() && (!currentFileEditor.IsDirty()))
                {
                    int currentTabIndex = tabControl.SelectedIndex;
                    currentFileEditor = newFileEditor;
                    fileEditors[currentTabIndex] = newFileEditor;
                    tabControl.SelectedTab.Text = openFileDialog1.SafeFileName;
                    tabControl.SelectedTab.Controls.Clear();
                    tabControl.SelectedTab.Controls.Add(currentFileEditor.getScintilla());
                }
                else
                {
                    TabPage newTab = new TabPage(openFileDialog1.SafeFileName);
                    tabControl.TabPages.Add(newTab);
                    currentFileEditor = newFileEditor;
                    fileEditors.Add(newFileEditor);
                    newTab.Controls.Add(currentFileEditor.getScintilla());
                    tabControl.SelectTab(fileEditors.Count - 1);
                }
                openFile.Close();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !CloseFile();

        }

        private bool CloseFile()
        {
            int i = 0;
            while (i < tabControl.TabCount)
            {
                currentFileEditor = (FileEditor)fileEditors[i];
                tabControl.SelectTab(i);
                if (currentFileEditor.IsDirty())
                {
                    if (currentFileEditor.IsNew())
                    {
                        DialogResult result = MessageBox.Show("Do you want to save this file?", "Saving new file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            if (saveAs() == false)
                                return false;
                        }
                        else if (result == DialogResult.Cancel)
                            return false;
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Do you want to save " + currentFileEditor.ShortName() + "?", "Saving file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                            save(currentFileEditor.Path());
                        else if (result == DialogResult.Cancel)
                            return false;
                    }
                }
                i++;
            }
            return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnNewFile();
        }

        private void tabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < tabControl.TabCount; i++)
                {
                    Rectangle r = tabControl.GetTabRect(i);
                    if (r.Contains(e.Location))
                    {
                        currentFileEditor = (FileEditor)fileEditors[i];
                        tabControl.SelectTab(i);
                        mnu.Show(tabControl, e.Location);
                    }
                }
            }
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            closeTab();
        }

        private void closeTab()
        {
            bool success = true;
            int currentTabIndex = tabControl.SelectedIndex;
            currentFileEditor = (FileEditor)fileEditors[currentTabIndex];
            if (currentFileEditor.IsDirty())
            {
                if (currentFileEditor.IsNew())
                {
                    DialogResult result = MessageBox.Show("Do you want to save " + currentFileEditor.ShortName() + "?", "Saving file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        success = saveAs();
                    else if (result == DialogResult.Cancel)
                        success = false;
                }
                else
                {
                    DialogResult result = MessageBox.Show("Do you want to save " + currentFileEditor.ShortName() + "?", "Saving file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        save(currentFileEditor.Path());
                    else if (result == DialogResult.Cancel)
                        success = false;
                }
            }
            if (success)
            {
                if (tabControl.TabCount == 1)
                    System.Environment.Exit(0);
                else
                {
                    fileEditors.RemoveAt(currentTabIndex);
                    tabControl.SelectedTab.Dispose();
                    if (currentTabIndex == tabControl.TabCount)
                    {
                        tabControl.SelectTab(currentTabIndex - 1);
                    }
                    else
                        tabControl.SelectTab(currentTabIndex);
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string address = "127.0.0.1";
            int port = 3002;
          
            client = new Client(address, port, this);
            if (client.connect())
            {
                connectButton.Enabled = false;
                uploadButton.Enabled = true;
                stepButton.Enabled = true;
            }
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            breakPoints = currentFileEditor.getBreakpoints();
            client.sendMessage(currentFileEditor.GetText() + "\0");
            client.sendMessage("Steps: ");
            for (int i = 0; i < breakPoints.Count; i++)
            {
                client.sendMessage("," + breakPoints[i]);
            }

        }

        private void stepButton_Click(object sender, EventArgs e)
        {
            client.sendMessage("Time to step");
        }

        public void displayIncomingText(string msg)
        {
            outputBox.Text += msg + "\r\n";
            outputBox.SelectionStart = statusBox.Text.Length;
            outputBox.ScrollToCaret();
        }

        public void displayStatusText(string msg)
        {
            string richMsg = ">> " + msg + "\r\n";
            statusBox.SelectionColor = Color.Black;
            statusBox.SelectedText = richMsg;
            statusBox.SelectionStart = statusBox.Text.Length;
            statusBox.ScrollToCaret();
        }

        public void displayStatusText(string msg, string type)
        {
            if (type.ToLower().Equals("warning")) writeColorText(statusBox, msg, Color.Red);
            else if (type.ToLower().Equals("info")) writeColorText(statusBox, msg, Color.DarkOrange);
            else if (type.ToLower().Equals("status")) writeColorText(statusBox, msg, Color.Green);
            else displayStatusText(msg);
        }

        private void writeColorText(RichTextBox txt, String msg, Color c)
        {
            string richMsg = ">> " + msg + "\r\n";
            txt.SelectionColor = c;
            txt.SelectedText = richMsg;
            txt.SelectionStart = statusBox.Text.Length;
            txt.ScrollToCaret();
        }
    }
}
