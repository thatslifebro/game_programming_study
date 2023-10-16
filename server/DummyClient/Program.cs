using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text;
using ServerCore;

namespace DummyClient
{
    

    class Program
    {

        static void Main(string[] args)
        {
            //DNS
            //string host = Dns.GetHostName();
            //IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress ipAddr = ipHost.AddressList[0];
            //IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 8888);

            Thread.Sleep(1000);

            Connector connector = new Connector();

            connector.Connect(endPoint, SessionManager.Instance.Generate, 1) ;


            Thread.Sleep(20000);
            SessionManager.Instance.SendForEach();
            //while (true)
            //{
            //    try
            //    {
            //        SessionManager.Instance.SendForEach();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.ToString());
            //    }

            //    Thread.Sleep(250);
            //}

            while (true)
            {
                Thread.Sleep(100000);
            }



        }

    }
}
//socket programming ex