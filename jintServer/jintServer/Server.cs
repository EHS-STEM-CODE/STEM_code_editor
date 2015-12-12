using System;
using System.Net;
using System.Net.Sockets;
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

			serverSocket.Start ();
			messageDisplay.displayStatusText ("Server Started ...");
			running = true;

			while (running) {
				if (serverSocket.Pending ()) {
					clientSocket = serverSocket.AcceptTcpClient ();
					messageDisplay.displayStatusText ("Client Connected.");

				}
			}
		}
	}
}

