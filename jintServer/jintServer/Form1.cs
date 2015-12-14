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
	public interface TwoWayMessageDisplay{
		void displayIncomingText (string msg);
		void displayStatusText(string msg);
	}

    public partial class Form1 : Form, TwoWayMessageDisplay
    {
		private Server server = null;

        public Form1()
        {
            InitializeComponent();
			string address = "127.0.0.1";
			int port = 3002;
			server = new Server (address, port, this);
            server.listen();
        }

		public void displayIncomingText(string msg){
			codeBox.Text = msg;
		}

		public void displayStatusText(string msg){
			outputBox.Text += msg;
		}
    }
}
