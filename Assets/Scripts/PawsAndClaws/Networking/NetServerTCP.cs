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

        public static Action<NetworkSocket> OnConnectionAccept;
        public static Action<NetworkSocket> OnClientDisconnect;

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

        void ReceiveJob(NetworkSocket clientSocket)
        {
            while (true)
            {
                int rbytes = ReceivePacket(clientSocket);
                Debug.Log($"Server received packet from client {clientSocket.IPAddrStr} with size {rbytes}");
                if (rbytes == 0)
                {
                    lock (_clientMutex)
                    {
                        Debug.Log($"Disconnected client from IP [{clientSocket.IPAddr}]");
                        OnClientDisconnect?.Invoke(clientSocket);
                        _connectedClients.Remove(clientSocket);
                    }
                    break;
                }
            }
        }

        void AcceptConnections()
        {
            Socket client = _serverSocket.Socket.Accept();

            string ipAddrStr = client.RemoteEndPoint.ToString();

            IPAddress addr = ((IPEndPoint)client.RemoteEndPoint).Address;
            int port = ((IPEndPoint)client.RemoteEndPoint).Port;

            Debug.Log($"Client connected from IP [{addr}] and port [{port}]");

            IPAddress clientAddr = IPAddress.Parse(addr.ToString());

            NetworkSocket clientSocket = new NetworkSocket(client, clientAddr, ipAddrStr);
            OnConnectionAccept?.Invoke(clientSocket);

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

        public override int SendPacket(object packet, NetworkSocket socket)
        {
            OnPacketSend?.Invoke();
            PacketBytes = Utils.BinaryUtils.ObjectToByteArray(packet);
            Debug.Log($"Server sent packet to IP: {socket.IPAddr}");
            return socket.Socket.Send(PacketBytes);
        }
    }
}