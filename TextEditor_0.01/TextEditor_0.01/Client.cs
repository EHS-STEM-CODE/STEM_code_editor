using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEditor_0._01
{
    class Client
    {
        ClientMessageDisplay display;

        public Client(string address, int port, ClientMessageDisplay d)
        {
            display = d;
            display.displayStatusText("Waiting","warning");
            for (int i = 0; i < 100; i++) display.displayStatusText("To much text","status");
            display.displayStatusText("info","info");
        }

       /* private void connect()
        {
            display.displayStatusText("Client Started");
            clientSocket.Connect(address, port);
            messageDisplay.displayStatusText("Client connected to Server.");
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
            display.displayIncomingText(returndata.Trim());
        }*/






    }
}
