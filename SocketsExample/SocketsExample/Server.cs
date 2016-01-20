using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

// from server example: https://msdn.microsoft.com/en-us/library/fx6588te%28v=vs.110%29.aspx

namespace SocketsExample
{
    class Server
    {
        private static string address = "127.0.0.1";
        private static int port;
        private static bool running;
        TwoWayMessageDisplay messageDisplay;

        public Server(string anAddress, int aPort, TwoWayMessageDisplay display)
        {
            address = anAddress;
            port = aPort;
            messageDisplay = display;
        }

        public void stop()
        {
            running = false;
        }

        public void listen()
        {
            IPAddress ipAddress = IPAddress.Parse(address);
            TcpListener serverSocket = new TcpListener(ipAddress, port);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            //messages should go through to the screen
            messageDisplay.displayStatusText("Server Started");

            counter = 0;
            running = true;
            //this blocks right here, waiting for a connection
            //would have to run this in a separate thread.
            while (running)
            {
                if (serverSocket.Pending())
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();
                    messageDisplay.displayStatusText("Client No:" + Convert.ToString(counter) + " started!");
                    ClientHandler client = new ClientHandler();
                    client.setMessageDisplay(messageDisplay);
                    client.startClient(clientSocket, Convert.ToString(counter));
                }
                Thread.Sleep(100);
            }

            serverSocket.Stop();
            messageDisplay.displayStatusText("server exited");
        }
    }

    public class ClientHandler
    {
        TcpClient clientSocket;
        bool running;
        string clNo;
        TwoWayMessageDisplay messageDisplay;

        public void startClient(TcpClient inClientSocket, string clientNumber)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clientNumber;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        public void setMessageDisplay(TwoWayMessageDisplay display)
        {
            messageDisplay = display;
        }

        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            running = true;
            while (running)
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf('\0'));
                    string msg = "(" + clNo + ") " + dataFromClient.Trim();
                    messageDisplay.displayIncomingText(msg);

                    rCount = Convert.ToString(requestCount);
                    serverResponse = "ACK:(" + clNo + ")." + rCount;
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    messageDisplay.displayOutgoingText(serverResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                    running = false;
                    clientSocket.Close();
                }
            }
        }
    } 
}