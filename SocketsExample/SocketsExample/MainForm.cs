using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using System.Threading;

namespace SocketsExample
{

    public interface TwoWayMessageDisplay
    {
        void displayIncomingText(string msg);
        void displayOutgoingText(string msg);
        void displayStatusText(string msg);
    }

    public partial class MainForm : Form, TwoWayMessageDisplay
    {
        private Server server = null;
        private Client client = null;
        private static string newline = "\r\n";
        private string appName;
        private enum AppType {UNKNOWN, CLIENT, SERVER};
        private AppType appType;

        public MainForm()
        {
            InitializeComponent();
            appName = System.AppDomain.CurrentDomain.FriendlyName;

            if (appName.ToLower().Contains("client"))
            {
                appType = AppType.CLIENT;
                radServer.Hide();
                radClient.Checked = true;
            }
            else if (appName.ToLower().Contains("server"))
            {
                appType = AppType.SERVER;
                radClient.Hide();
                radServer.Checked = true;
            }

        }

        private void textOutgoing_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                client.sendMessage(textOutgoing.Text);
                textOutgoing.Text = "";
                e.Handled = true;
            }

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            string address = "127.0.0.1";
            int port = 3002;
            if (radClient.Checked)
            {
                client = new Client(address, port, this);
                displayStatusText("Client started.");
            }
            else
            {
                server = new Server(address, port, this);
                //start a separate thread for the server
                Thread ctThread = new Thread(server.listen);
                ctThread.Start();
                buttonStart.Enabled = false;
            }
        }

        private void buttonStop_Click_1(object sender, EventArgs e)
        {
            if (radServer.Checked && server != null)
                server.stop();
        }

        delegate void SetTextCallBack(string msg);

        //handles requests coming for other threads, requiring invoke.
        public void displayIncomingText(string msg)
        {
            if (textIncoming.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(displayIncomingText);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.textIncoming.AppendText(msg + newline);
            }
        }

        public void displayOutgoingText(string msg)
        {
            if (textOutgoing.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(displayOutgoingText);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.textOutgoing.AppendText(msg + newline);
            }
        }

        public void displayStatusText(string msg)
        {
            if (lblStatus.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(displayStatusText);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.lblStatus.Text = msg;
            }
        }

    }
}
