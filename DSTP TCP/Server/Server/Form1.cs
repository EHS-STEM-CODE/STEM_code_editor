using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Server
{
    public partial class Form1 : Form
    {
        TcpListener serverSocket = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 3002);
        NetworkStream serverStream;
        TcpClient clientSocket;
        public Form1()
        {
            InitializeComponent();
            serverSocket.Start();
            Console.Write("Waiting to connect to a client");
            clientSocket = serverSocket.AcceptTcpClient();
            writeString("Server test message");
            textBox1.Text = readString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                
        }

        private void readButton(object sender, EventArgs e)
        {
            String str = readString();
            if (str != null)
            {
                if (str == "!")
                    textBox1.ForeColor = Color.Salmon;
                else if (str == "@")
                    textBox1.ForeColor = Color.MidnightBlue;
                else if (str == "#")
                    textBox1.ForeColor = Color.Ivory;
                else
                    textBox1.Text = str;
            }
        }
        private void sendButton(object sender, EventArgs e)
        {
            writeString(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            writeString("#");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            writeString("@");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            writeString("!");
        }
        private void writeString(String sending)
        {
            try
            {
                serverStream = clientSocket.GetStream();
                byte[] outStream = Encoding.ASCII.GetBytes(sending);
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();   //Send
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("IO Exception");
            }
        }

        private String readString()
        {
            String returnString = null;
            try
            {
                serverStream = clientSocket.GetStream();
                byte[] inStream = new byte[10025];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                returnString = System.Text.Encoding.ASCII.GetString(inStream);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Socket error");
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("IO exception");
            }
            return (returnString);
        }
    }
}
