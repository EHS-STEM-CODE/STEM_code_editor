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

        TcpClient clientSocket = new TcpClient();

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
                NetworkStream serverStream = clientSocket.GetStream();

                byte[] inStream = new byte[10025];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                string returndata = Encoding.ASCII.GetString(inStream);
                returndata = returndata.Substring(0, returndata.IndexOf('\0')); //strip nulls
                messageDisplay.displayIncomingText(returndata.Trim());
                serverStream.Flush();
                return true;
            }
            catch(SocketException)
            {
                messageDisplay.displayStatusText("Unable to connect to server @ port: " + port + " Address: " + address, "warning");
                return (false);
            }
        }


        public void sendMessage(string msg)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = Encoding.ASCII.GetBytes(msg);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[10025];
            serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returndata = Encoding.ASCII.GetString(inStream);
            returndata = returndata.Substring(0, returndata.IndexOf('\0')); //strip nulls
            messageDisplay.displayIncomingText(returndata.Trim());
            serverStream.Flush();
        }
    }
}
