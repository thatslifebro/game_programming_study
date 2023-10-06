using System.Collections;
using System.Collections.Generic;
using ServerCore;
using System.Net;
using System.Threading;
using UnityEngine;
using DummyClient;
using System;

public class NetworkManager : MonoBehaviour
{
    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }
    // Start is called before the first frame update
    ServerSession _session = new ServerSession();
    void Start()
    {
        //string host = Dns.GetHostName();
        //IPHostEntry ipHost = Dns.GetHostEntry(host);
        //IPAddress ipAddr = ipHost.AddressList[0];
        //IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 8888);

        Debug.Log(endPoint);

        Thread.Sleep(1000);

        Connector connector = new Connector();

        connector.Connect(endPoint, () => { return _session; });

        
    }

    // Update is called once per frame
    void Update()
    {
        List<IPacket> list = PacketQueue.Instance.PopAll();
        foreach(IPacket packet in list)
        {
            PacketManager.Instance.HandlePacket(_session, packet);
        }
    }

    
}
