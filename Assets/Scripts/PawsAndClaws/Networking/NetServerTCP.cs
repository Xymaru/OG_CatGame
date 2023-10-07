using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class NetServerTCP : MonoBehaviour
{
    private NetworkManager _networkManager;
    private TMPro.TextMeshProUGUI _ipsText;
    private NetworkServerSocket _serverSocket;

    private Thread _acceptThread;

    private readonly List<NetworkSocket> _connectedClients = new List<NetworkSocket>();
    private readonly List<Thread> _clientThreads = new List<Thread>();

    private readonly object _clientMutex = new object();

    void Start()
    {
        _networkManager = GetComponent<NetworkManager>();
        _ipsText = _networkManager.ipsText;
        _serverSocket = (NetworkServerSocket)NetworkData.NetSocket;
        _serverSocket.Socket.Listen(10);

        // Accept incoming connections job
        _acceptThread = new Thread(AcceptJob);
        _acceptThread.Start();
    }

    void Update()
    {
        UpdateIPList();
    }

    void UpdateIPList()
    {
        _ipsText.text = "Connected IPs\n";

        lock (_clientMutex)
        {
            foreach (var client in _connectedClients)
            {
                _ipsText.text += $"{client.IPAddrStr}\n";
            }
        }
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
            int rbytes = clientsocket.Socket.Receive(data);

            if (rbytes == 0)
            {
                break;
            }

            string msg = Encoding.ASCII.GetString(data);

            Debug.Log($"Received message [{msg}] from IP [{clientsocket.IPAddr}]");
        }
    }

    void AcceptConnections()
    {
        Socket client = _serverSocket.Socket.Accept();

        string ipaddr_str = client.RemoteEndPoint.ToString();

        IPAddress addr = ((IPEndPoint)client.RemoteEndPoint).Address;
        int port = ((IPEndPoint)client.RemoteEndPoint).Port;

        Debug.Log("Client connected from IP [" + addr.ToString() + "] and port [" + port + "]");

        IPAddress clientaddr = IPAddress.Parse(addr.ToString());

        NetworkSocket clientnsocket = new NetworkSocket(client, clientaddr, ipaddr_str);

        Thread clientthread = new Thread(() => ReceiveJob(clientnsocket));

        lock (_clientMutex)
        {
            _connectedClients.Add(clientnsocket);
            _clientThreads.Add(clientthread);
        }

        clientthread.Start();
    }

    private void OnDestroy()
    {
        if (_acceptThread.IsAlive)
        {
            _acceptThread.Abort();

            if (_serverSocket.Socket.Connected)
            {
                _serverSocket.Socket.Shutdown(SocketShutdown.Both);
            }

            _serverSocket.Socket.Close();
        }

        for (int i = 0; i < _connectedClients.Count; i++)
        {
            NetworkSocket clientsock = _connectedClients[i];

            if (clientsock.Socket.Connected)
            {
                clientsock.Socket.Shutdown(SocketShutdown.Both);
            }

            clientsock.Socket.Close();

            Thread thr = _clientThreads[i];

            if (thr.IsAlive)
            {
                thr.Abort();
            }
        }
    }
}