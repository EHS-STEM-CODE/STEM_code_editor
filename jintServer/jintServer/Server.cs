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
					byte[] bytesFrom = new byte[10005];
					string dataFromClient = null;
					Byte[] sendBytes = null;

					while (running) {
						try{
							NetworkStream stream = clientSocket.GetStream();
							stream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
							dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
							dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf('\0'));
						}catch(Exception e){
							Console.WriteLine (" >> " + e.ToString ());
							running = false;
							clientSocket.Close ();
						}
					}
				}
			}
		}
	}
}

