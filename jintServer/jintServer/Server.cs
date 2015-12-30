using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Jint;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace jintServer
{
    public class Server
    {
        private static string address;
        private static int port;
        private static bool running;
        TwoWayMessageDisplay messageDisplay;
        List<ClientHandler> handles;
        public static Form1 form;

        public Server(string anAddress, int aPort, TwoWayMessageDisplay display, Form1 _form)
        {
            address = anAddress;
            port = aPort;
            messageDisplay = display;
            handles = new List<ClientHandler>();
            form = _form;
        }

        public void stop() {
            running = false;
        }

        public void listen() {
            IPAddress ipAddress = IPAddress.Parse(address);
            TcpListener serverSocket = new TcpListener(ipAddress, port);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            messageDisplay.displayStatusText("Server Started ...");
            running = true;
            counter = 0;

            while (running) {
                if (serverSocket.Pending())
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();
                    messageDisplay.displayStatusText("Client No: " + Convert.ToString(counter) + " started");
                    ClientHandler client = new ClientHandler();
                    handles.Add(client);
                    client.setMessageDisplay(messageDisplay);
                    client.startClient(clientSocket, Convert.ToString(counter));
                }
                Thread.Sleep(100);
            }
            foreach (ClientHandler h in handles)
            {
                h.stop();
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
                outputMessage += "s is null\n";
            else
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
            networkStream.ReadTimeout = 50;
            serverResponse = "ACK:(" + clNo + ")." + rCount + "\0";
            sendBytes = Encoding.ASCII.GetBytes(serverResponse);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();

            //var engine = new Engine().SetValue("log", new Action<object>(getString));
            var engine = new Engine(cfg => cfg.AllowClr());
            engine.SetValue("log", new Action<object>(getString));
            engine.SetValue("Color", new System.Drawing.Color());
            indy ui = new indy();
            engine.SetValue("ui", ui);
            running = true;
            while (running)
            {
                try
                {
                    requestCount = requestCount + 1;
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf('\0'));
                    messageDisplay.displayStatusText("Received code from client No. " + clNo);

                    rCount = Convert.ToString(requestCount);
                    outputMessage = "ok\n";
                    try
                    {
                        engine.Execute(dataFromClient);
                    }
                    catch (Exception e)
                    {
                        outputMessage = e.ToString();
                    }
                    sendBytes = Encoding.ASCII.GetBytes(outputMessage);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    messageDisplay.displayStatusText("Output sent to client No. " + clNo);
                    messageDisplay.displayOutgoingText(outputMessage);
                }
                catch (System.IO.IOException ex)
                {
                    // expected time out
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                    running = false;
                }
            }
            clientSocket.Close();
        }

        public void stop()
        {
            running = false;
        }
    }

    
    public class indy
    {
        private bool On = true;
        public indy()
        {

        }

      

        public void setColor(Color c)
        {
            Server.form.changeButtonColor(c);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
