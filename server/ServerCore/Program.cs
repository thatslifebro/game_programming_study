using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerCore
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

            Socket listenSocket = new Socket(endPoint.AddressFamily,SocketType.Stream,ProtocolType.Tcp);

            try
            {
                listenSocket.Bind(endPoint);

                listenSocket.Listen(10); //동시 접속

                while (true)
                {
                    Console.WriteLine("Listenning...");

                    Socket clientSocket = listenSocket.Accept(); //blocking 넘어가지 못하면 대기 게임에선 blocking으로 처리되면 안됌 

                    //recv
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = clientSocket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine($"from client : {recvData}");

                    //send
                    byte[] sendBuff = Encoding.UTF8.GetBytes("welcome to server!");
                    clientSocket.Send(sendBuff);

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

}

//socket programming ex