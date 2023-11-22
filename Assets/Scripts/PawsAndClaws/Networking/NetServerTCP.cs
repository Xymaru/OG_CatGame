using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace PawsAndClaws.Networking
{
    public class NetServerTCP : NetServer
    {
        private PacketManagerTCP _packetManager = new PacketManagerTCP();
        private const int MAX_CONNECTIONS = 6;

        void Start()
        {
            // Listen to a maximum of 10 connections
            _serverSocket.Socket.Listen(10);
            
            Debug.Log($"Server IP: {_serverSocket.IPAddrStr} listening on port: {NetworkData.Port}");

            _packetManager.OnSocketDisconnected += OnSocketDisconnected;
            _packetManager.OnPacketReceived += OnReceivedPacket;
            
            // Begin accepting connections
            BeginAccept();
        }
        
        public void BroadcastPacket(NetworkPacket packet)
        {
            byte[] data = packet.ToByteArray();

            foreach(NetworkSocket socket in ConnectedClients)
            {
                if(socket != null)
                {
                    socket.Socket.Send(data, NetworkPacket.MAX_BUFFER_SIZE, 0);
                }
            }
        }

        protected void BeginAccept()
        {
            PacketState packetState = new PacketState();
            packetState.socket = _serverSocket;
            _serverSocket.Socket.BeginAccept(new AsyncCallback(AcceptCB), packetState);
        }

        void AcceptCB(IAsyncResult ar)
        {
            PacketState packetState = (PacketState)ar.AsyncState;

            NetworkServerSocket srv = (NetworkServerSocket)packetState.socket;

            Socket client = srv.Socket.EndAccept(ar);

            string ipAddrStr = client.RemoteEndPoint.ToString();

            IPAddress addr = ((IPEndPoint)client.RemoteEndPoint).Address;
            int port = ((IPEndPoint)client.RemoteEndPoint).Port;

            Debug.Log($"Client connected from IP [{addr}] and port [{port}]");

            IPAddress clientAddr = IPAddress.Parse(addr.ToString());

            NetworkSocket clientSocket = new NetworkSocket(client, clientAddr, ipAddrStr);

            Packets.NPLobbyRes lres = new();

            if (ConnectedClients.Count == MAX_CONNECTIONS)
            {
                lres.response = ResponseType.LOBBY_FULL;

                byte[] data = lres.ToByteArray();

                clientSocket.Socket.Send(data, NetworkPacket.MAX_BUFFER_SIZE, 0);
            }
            else
            {
                clientSocket.PlayerI.client_id = Convert.ToUInt16(ConnectedClients.Count);

                lres.response = ResponseType.ACCEPTED;
                lres.player_id = clientSocket.PlayerI.client_id;

                byte[] data = lres.ToByteArray();

                // Send lobby response
                clientSocket.Socket.Send(data, NetworkPacket.MAX_BUFFER_SIZE, 0);

                // Begin receiving data from client
                _packetManager.BeginReceive(clientSocket);

                // Add client to list
                ConnectedClients.Add(clientSocket);

                // Execute callbacks
                OnConnectionAccept?.Invoke(clientSocket);
            }

            // Keep accepting connections
            _serverSocket.Socket.BeginAccept(new AsyncCallback(AcceptCB), packetState);
        }

        void OnSocketDisconnected(NetworkSocket socket)
        {
            // Remove client from list
            ConnectedClients.Remove(socket);

            // On client disconnect callback
            OnClientDisconnect?.Invoke(socket);

            Debug.Log($"Client disconnected from IP {socket.IPAddr}");
        }

        void OnReceivedPacket(NetworkPacket packet)
        {
            NetworkManager.OnPacketReceived?.Invoke(packet);
        }

        private void OnDestroy()
        {
            if (_serverSocket.Socket.Connected)
            {
                _serverSocket.Socket.Shutdown(SocketShutdown.Both);
            }

            _serverSocket.Socket.Close();

            for (int i = 0; i < ConnectedClients.Count; i++)
            {
                NetworkSocket clientsock = ConnectedClients[i];

                if (clientsock.Socket.Connected)
                {
                    clientsock.Socket.Shutdown(SocketShutdown.Both);
                }

                clientsock.Socket.Close();
            }
        }
    }
}