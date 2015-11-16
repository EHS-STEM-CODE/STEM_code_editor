using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace server
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try{
				TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 3000);
				server.Start();
				Console.WriteLine("Server is connecting to port 3000...");
				Console.WriteLine("Server is waiting for a connection...");

				Socket socket = server.AcceptSocket();
				Console.WriteLine("Connection accepted from " + socket.RemoteEndPoint);

				while(true){
					byte[] msg = new byte[1024];
					int msgLength = socket.Receive(msg);
					for(int i = 0; i < msgLength; i++)
						Console.Write(Convert.ToChar(msg[i]));
					Console.WriteLine();

					ASCIIEncoding asen = new ASCIIEncoding();
					socket.Send(asen.GetBytes("Message sent!"));
				}
			}catch(Exception e){
				Console.WriteLine ("Error... " + e.StackTrace);
			}
		}
	}
}
