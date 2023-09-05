﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace DummyClient
{

    class Program
    {

        static void Main(string[] args)
        {
            //DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                Thread.Sleep(100);
                socket.Connect(endPoint);
                Console.WriteLine($"connect to {socket.RemoteEndPoint.ToString()}");

                //send
                byte[] sendBuff = Encoding.UTF8.GetBytes("hello this is client");
                int sendBytes = socket.Send(sendBuff);

                //recv
                byte[] recvBuff = new byte[1024];
                int recvBytes = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Console.WriteLine($"from server : {recvData.ToString()}");

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            

        }

    }
}
//socket programming ex