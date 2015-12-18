using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Jint;

namespace jintServer
{
	public class Server
	{
		private static string address;
		private static int port;
		private static bool running;
		TwoWayMessageDisplay messageDisplay;

		public Server (string anAddress, int aPort, TwoWayMessageDisplay display)
		{
			address = anAddress;
			port = aPort;
			messageDisplay = display; 
		}

		public void stop(){
			running = false;
		}

		public void listen(){
			IPAddress ipAddress = IPAddress.Parse (address);
			TcpListener serverSocket = new TcpListener (ipAddress, port);
			TcpClient clientSocket = default(TcpClient);
            int counter = 0;

			serverSocket.Start ();
			messageDisplay.displayStatusText ("Server Started ...");
			running = true;
            counter = 0;

			while (running) {
				if (serverSocket.Pending())
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();
                    messageDisplay.displayStatusText("Client No: " + Convert.ToString(counter) + " started");
                    ClientHandler client = new ClientHandler();
                    client.setMessageDisplay(messageDisplay);
                    client.startClient(clientSocket, Convert.ToString(counter));
                }
                Thread.Sleep(100);
			}
            serverSocket.Stop();
		}
	}

    public class ClientHandler
    {
        TcpClient clientSocket;
        bool running;
        string clNo;
        TwoWayMessageDisplay messageDisplay;
        string outputMessage;

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

        private void getString(object s)
        {
            if (s == null)
                outputMessage += "null";
            outputMessage += s.ToString() + "\n";
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

            NetworkStream networkStream = clientSocket.GetStream();
            serverResponse = "ACK:(" + clNo + ")." + rCount + "\0";
            sendBytes = Encoding.ASCII.GetBytes(serverResponse);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();

            var engine = new Engine().SetValue("log", new Action<object>(getString));
            running = true;
            while (running)
            {
                try
                {
                    requestCount = requestCount + 1;
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf('\0'));
                    string msg = "(" + clNo + ") " + dataFromClient.Trim();
                    messageDisplay.displayIncomingText(msg);

                    rCount = Convert.ToString(requestCount);

                    try
                    {
                        engine.Execute(dataFromClient);
                    }catch(Exception e)
                    {
                        outputMessage = e.ToString();
                    }
                    sendBytes = Encoding.ASCII.GetBytes(outputMessage);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    messageDisplay.displayStatusText(serverResponse);
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