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
            textBox1.Text = (System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)).ToString();
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

        delegate void SetColorCallBack(Color c);
        public void ChangeButtonColor(Color c)
        { 
            if (outputBox.InvokeRequired)
            {
                SetColorCallBack d = new SetColorCallBack(ChangeButtonColor);
                this.Invoke(d, new object[] { c });
            }
            else
            {
                this.button1.BackColor = c;
            }
        }

        public void ChangeButtonTextColor(Color c)
        {
            if (outputBox.InvokeRequired)
            {
                SetColorCallBack d = new SetColorCallBack(ChangeButtonTextColor);
                this.Invoke(d, new object[] { c });
            }
            else
            {
                this.button1.ForeColor = c;
            }
        }

        public void ChangeButtonText(String txt)
        {
            if (outputBox.InvokeRequired)
            {
                SetTextCallBack d = new SetTextCallBack(ChangeButtonText);
                this.Invoke(d, new object[] { txt });
            }
            else
            {
                this.button1.Text = txt;
            }
        }

        private void ListenButton_Click(object sender, EventArgs e)
        {
           
            try {
                
                string address = "127.0.0.1";
                int port = Int32.Parse(textBox2.Text);
                server = new Server(address, port, this, this);

                Thread ctThread = new Thread(server.listen);
                ctThread.Start();

                ListenButton.Enabled = false;
                StopButton.Enabled = true;
            }
            catch(System.FormatException)
            {
                displayStatusText("Invalid path");
            }
                   
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            server.stop();
            StopButton.Enabled = false;
            ListenButton.Enabled = true;
        }


    }
}
