using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PawsAndClaws.Networking
{
    public class NetServerTCP : NetServer
    {
        private TMPro.TextMeshProUGUI _ipsText;

        private Thread _acceptThread;

        private readonly List<NetworkSocket> _connectedClients = new();
        private readonly List<Thread> _clientThreads = new();

        private readonly object _clientMutex = new();
        void Start()
        {
            _ipsText = _networkManager.ipsText;
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
            while (true)
            {
                int rbytes = ReceivePacket(clientsocket);

                if (rbytes == 0)
                {
                    lock (_clientMutex)
                    {
                        //_lastMessagesReceived.Add($"Disconnected client from IP [{clientsocket.IPAddr}]");
                        _connectedClients.Remove(clientsocket);
                    }
                    break;
                }

                lock (_clientMutex)
                {
                    //_lastMessagesReceived.Add(Encoding.ASCII.GetString(data));
                }
            }
        }

        void AcceptConnections()
        {
            Socket client = _serverSocket.Socket.Accept();

            string ipAddrStr = client.RemoteEndPoint.ToString();

            IPAddress addr = ((IPEndPoint)client.RemoteEndPoint).Address;
            int port = ((IPEndPoint)client.RemoteEndPoint).Port;
            //_lastMessagesReceived.Add("Client connected from IP [" + addr.ToString() + "] and port [" + port + "]");
            IPAddress clientAddr = IPAddress.Parse(addr.ToString());

            NetworkSocket clientSocket = new NetworkSocket(client, clientAddr, ipAddrStr);

            Thread clientThread = new Thread(() => ReceiveJob(clientSocket));

            lock (_clientMutex)
            {
                _connectedClients.Add(clientSocket);
                _clientThreads.Add(clientThread);
            }

            clientThread.Start();
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

        protected override int ReceivePacket(NetworkSocket socket)
        {
            OnPacketReceived?.Invoke();
            return socket.Socket.Receive(PacketBytes);
        }

        protected override int SendPacket(object packet, NetworkSocket socket)
        {
            OnPacketSend?.Invoke();
            PacketBytes = Utils.BinaryUtils.ObjectToByteArray(packet);
            return socket.Socket.Send(PacketBytes);
        }
    }
}