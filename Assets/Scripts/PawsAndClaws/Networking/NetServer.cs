using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading;

public class NetServer : MonoBehaviour
{
    NetworkServerSocket serversocket;

    Thread m_AcceptThread;

    List<NetworkSocket> m_ConnectedClients = new List<NetworkSocket>();

    object m_ClientMutex = new object();

    void Start()
    {
        serversocket = (NetworkServerSocket)NetworkData.netSocket;
        serversocket.socket.Listen(10);

        // Accept incoming connections job
        m_AcceptThread = new Thread(AcceptJob);
        m_AcceptThread.Start();


    }

    void Update()
    {

    }

    void AcceptJob()
    {
        // Loop to listen for connections
        while (true)
        {
            AcceptConnections();
        }
    }

    void AcceptConnections()
    {
        Socket client = serversocket.socket.Accept();

        string ipaddr_str = client.RemoteEndPoint.ToString();

        IPAddress addr = ((IPEndPoint)client.RemoteEndPoint).Address;
        int port = ((IPEndPoint)client.RemoteEndPoint).Port;

        Debug.Log("Client connected from IP [" + addr.ToString() + "] and port [" + port + "]");

        IPAddress clientaddr = IPAddress.Parse(addr.ToString());

        lock (m_ClientMutex)
        {
            m_ConnectedClients.Add(new NetworkSocket(client, clientaddr, ipaddr_str));
        }
    }

    private void OnDestroy()
    {
        if (m_AcceptThread.IsAlive)
        {
            m_AcceptThread.Abort();

            serversocket.socket.Shutdown(SocketShutdown.Both);
            serversocket.socket.Close();
        }
    }
}
