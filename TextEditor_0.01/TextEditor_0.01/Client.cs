using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TextEditor_0._01
{
    class Client
    {
        private static string address = "127.0.0.1";
        private static int port;

        ClientMessageDisplay messageDisplay;

        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        public Client(string anAddress, int aPort, ClientMessageDisplay d)
        {
            address = anAddress;
            port = aPort;
            messageDisplay = d;
        }

        public bool connect()
        {
            try
            {
                messageDisplay.displayStatusText("Client Started","info");
                clientSocket.Connect(address, port);
                messageDisplay.displayStatusText("Client connected to Server","status");
                return (true);
            }
            catch(System.Net.Sockets.SocketException)
            {
                messageDisplay.displayStatusText("Unable to connect to server @ port: " + port + " Address: " + address, "warning");
                return (false);
            }
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
            messageDisplay.displayIncomingText(returndata.Trim());
            serverStream.Flush();
        }
    }
}
