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
        private string output;

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
                    var engine = new Engine().SetValue("log", new Action<object>(getString));
					while (running) {
						try{
                            output = "";
							NetworkStream stream = clientSocket.GetStream();
							stream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
							dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
							dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf('\0'));
                            engine.Execute(dataFromClient);
                            sendBytes = System.Text.Encoding.ASCII.GetBytes(output);
                            stream.Write(sendBytes, 0, sendBytes.Length);
                            stream.Flush();
                            messageDisplay.displayStatusText(output);
						}catch(Exception e){
							Console.WriteLine (" >> " + e.ToString ());
							running = false;
							clientSocket.Close ();
						}
					}
				}
			}
		}

        private void getString(object s)
        {
            if (s == null)
                output += "null";
            output += s.ToString();
        }

	}
}

