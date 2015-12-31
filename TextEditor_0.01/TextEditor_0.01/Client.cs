using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TextEditor_0._01
{
    class Client
    {
        private static string address;
        private static int port;
        private int messageCount;

        ClientMessageDisplay messageDisplay;

        TcpClient clientSocket = new TcpClient();

        public Client(string anAddress, int aPort, ClientMessageDisplay d)
        {
            address = anAddress;
            port = aPort;
            messageDisplay = d;
            messageCount = 0;
        }

        public bool connect()
        {
            try
            {
                messageDisplay.DisplayStatusText("Client Started","info");
                clientSocket.Connect(address, port);
                messageDisplay.DisplayStatusText("Client connected to Server","status");
                NetworkStream serverStream = clientSocket.GetStream();

                byte[] inStream = new byte[10025];
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                string returndata = Encoding.ASCII.GetString(inStream);
                returndata = returndata.Substring(0, returndata.IndexOf('\0')); //strip nulls
                messageDisplay.DisplayIncomingText(returndata.Trim());
                serverStream.Flush();
                return true;
            }
            catch(SocketException)
            {
                messageDisplay.DisplayStatusText("Unable to connect to server @ port: " + port + " Address: " + address, "warning");
                return (false);
            }
        }


        public void sendMessage(string msg)
        {
            String ack = "ok_tx\n";
            NetworkStream serverStream = clientSocket.GetStream();
            try {
                byte[] outStream = Encoding.ASCII.GetBytes(msg);
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                messageCount++;
            }
            catch(System.IO.IOException)
            {
                messageDisplay.DisplayStatusText("Unable to connect to the server", "warning");
            } 
            messageDisplay.DisplayStatusText("Message #" + messageCount + " sent", "info");
            byte[] inStream = new byte[10025]; 
            try
            {
                serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                string returndata = Encoding.ASCII.GetString(inStream);
                returndata = returndata.Substring(0, returndata.IndexOf('\0')); //strip nulls

                if (returndata.Contains(ack))
                {
                    returndata = returndata.Substring(ack.Length);
                    messageDisplay.DisplayStatusText("Return message #" + messageCount + " received", "info");
                }

                messageDisplay.DisplayIncomingText(returndata.Trim());
                serverStream.Flush();
               
                
            }
            catch (System.IO.IOException ioex)
            {
                if (!clientSocket.Connected)
                    messageDisplay.DisplayStatusText("Lost the server connection", "warning");
                else
                    messageDisplay.DisplayStatusText("Unkown IO Exception " + ioex.Message, "warning");
            }
        }

        public bool isConnected()
        {
            return(clientSocket.Connected);
        }
    }
}
