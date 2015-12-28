using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace jintServer
{
	public interface TwoWayMessageDisplay{
		void displayOutgoingText (string msg);
		void displayStatusText(string msg);
	}

    public partial class Form1 : Form, TwoWayMessageDisplay
    {
        private Server server;

        public Form1()
        {
            InitializeComponent();
            StopButton.Enabled = false;
        }

        delegate void SetTextCallBack(string msg);

        public void displayOutgoingText(string msg){
            if (codeBox.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(displayOutgoingText);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.codeBox.Text = msg;
				codeBox.SelectionStart = codeBox.Text.Length;
				codeBox.ScrollToCaret ();
            }
        }

		public void displayStatusText(string msg){
            if (outputBox.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(displayStatusText);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.outputBox.Text += msg + "\n";
				outputBox.SelectionStart = outputBox.Text.Length;
				outputBox.ScrollToCaret ();
            }
        }       

        private void ListenButton_Click(object sender, EventArgs e)
        {
            ListenButton.Enabled = false;
            StopButton.Enabled = true;
            string address = "127.0.0.1";
            int port = 3002;
            server = new Server(address, port, this);
            Thread ctThread = new Thread(server.listen);
            ctThread.Start();
            
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            server.stop();
            StopButton.Enabled = false;
            ListenButton.Enabled = true;
        }
    }
}
