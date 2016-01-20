using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


//from client example: https://msdn.microsoft.com/en-us/library/bew39x2a%28v=vs.110%29.aspx

namespace SocketsExample
{
    class Client
    {

        private static string address = "127.0.0.1";
        private static int port;
        TwoWayMessageDisplay messageDisplay;

        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        public Client(string anAddress, int aPort, TwoWayMessageDisplay display)
        {
            address = anAddress;
            port = aPort;
            messageDisplay = display;
            connect();
        }

        private void connect()
        {
            messageDisplay.displayStatusText("Client Started");
            clientSocket.Connect(address, port);
            messageDisplay.displayStatusText("Client connected to Server.");
        }

        public void sendMessage(string msg)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(msg);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[10025];
            serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            returndata = returndata.Substring(0, returndata.IndexOf('\0')); //strip nulls
            messageDisplay.displayIncomingText( returndata.Trim());
        }
    }
    
}
