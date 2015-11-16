using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace client
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try{
				TcpClient client = new TcpClient();
				Console.WriteLine("Connecting...");
				client.Connect("127.0.0.1", 3000);
				Console.WriteLine("Connected successful");

				ASCIIEncoding asen = new ASCIIEncoding();
				Stream stm = client.GetStream();

				while(true){
					Console.Write(">> ");
					String str = Console.ReadLine();
					byte[] msg = asen.GetBytes(str);
					stm.Write(msg, 0, msg.Length);
					byte[] readBuffer = new byte[1024];
					int readBufferLength = stm.Read(readBuffer, 0, 1024);
					for(int i = 0; i < readBufferLength; i++)
						Console.Write(Convert.ToChar(readBuffer[i]));
					Console.WriteLine();
				}
			}catch(Exception e){
				Console.WriteLine ("Error... " + e.StackTrace);
			}
		}
	}
}
