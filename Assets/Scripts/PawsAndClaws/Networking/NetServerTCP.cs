using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading;

public class NetServerTCP : MonoBehaviour
{
    NetworkServerSocket serversocket;

    Thread m_AcceptThread;

    List<NetworkSocket> m_ConnectedClients = new List<NetworkSocket>();
    List<Thread> m_ClientThreads = new List<Thread>();

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

    void ReceiveJob(NetworkSocket clientsocket)
    {
        byte[] data = new byte[2048];

        while (true)
        {
            int rbytes = clientsocket.socket.Receive(data);

            if(rbytes == 0)
            {
                break;
            }

            string msg = Encoding.ASCII.GetString(data);

            Debug.Log("Received message [" + msg + "] from IP [" + clientsocket.ip_addr_str + "]");
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

        NetworkSocket clientnsocket = new NetworkSocket(client, clientaddr, ipaddr_str);

        Thread clientthread = new Thread(() => ReceiveJob(clientnsocket));
        
        lock (m_ClientMutex)
        {
            m_ConnectedClients.Add(clientnsocket);
            m_ClientThreads.Add(clientthread);
        }

        clientthread.Start();
    }

    private void OnDestroy()
    {
        if (m_AcceptThread.IsAlive)
        {
            m_AcceptThread.Abort();

            if (serversocket.socket.Connected)
            {
                serversocket.socket.Shutdown(SocketShutdown.Both);
            }
            serversocket.socket.Close();
        }

        for(int i = 0; i < m_ConnectedClients.Count; i++)
        {
            NetworkSocket clientsock = m_ConnectedClients[i];

            if (clientsock.socket.Connected)
            {
                clientsock.socket.Shutdown(SocketShutdown.Both);
            }
            clientsock.socket.Close();

            Thread thr = m_ClientThreads[i];

            if (thr.IsAlive)
            {
                thr.Abort();
            }
        }
    }
}
