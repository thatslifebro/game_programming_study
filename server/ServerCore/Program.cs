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
        static Listener _listener = new Listener();

        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                
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

            } catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void Main(string[] args)
        {
            //DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);


            _listener.init(OnAcceptHandler,endPoint);
            Console.WriteLine("Listenning...");

            while (true)
            {
                Thread.Sleep(1);

            }
            
        }
    }

}

//socket programming ex