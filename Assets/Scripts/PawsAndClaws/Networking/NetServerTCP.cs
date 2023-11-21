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
        void Start()
        {
            // Listen to a maximum of 10 connections
            _serverSocket.Socket.Listen(10);
            
            Debug.Log($"Server IP: {_serverSocket.IPAddrStr} listening on port: {NetworkData.Port}");
            
            // Begin accepting connections
            BeginAccept();
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

            // Begin receiving data from client
            BeginReceive(clientSocket);

            // Add client to list
            ConnectedClients.Add(clientSocket);

            // Execute callbacks
            OnConnectionAccept?.Invoke(clientSocket);

            // Keep accepting connections
            _serverSocket.Socket.BeginAccept(new AsyncCallback(AcceptCB), packetState);
        }

        protected void BeginReceive(NetworkSocket client)
        {
            PacketState packetState = new PacketState();
            packetState.socket = client;
            packetState.buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];
            packetState.bytesRead = 0;

            client.Socket.BeginReceive(packetState.buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, new AsyncCallback(ReceiveCB), packetState);
        }

        void ReceiveCB(IAsyncResult ar)
        {
            PacketState packetState = (PacketState)ar.AsyncState;
            Socket client = packetState.socket.Socket;

            int bytesRead = client.EndReceive(ar);

            // Client disconnected
            if(bytesRead == 0)
            {
                Debug.Log($"Disconnected client from IP [{packetState.socket.IPAddr}]");

                // Remove client from list
                ConnectedClients.Remove(packetState.socket);

                // On client disconnect callback
                OnClientDisconnect?.Invoke(packetState.socket);

                // Ignore data and don't continue receiving
                return;
            }

            // Add bytes read
            packetState.bytesRead += bytesRead;

            // Check if packet is completed
            if(packetState.bytesRead == NetworkPacket.MAX_BUFFER_SIZE)
            {
                packetState.bytesRead = 0;

                NetworkPacket npacket = NetworkPacket.ByteArrayToNetworkPacket(packetState.buffer);

                OnPacketReceived?.Invoke(npacket);
            }

            // Continue reading in offset
            client.BeginReceive(packetState.buffer, packetState.bytesRead, NetworkPacket.MAX_BUFFER_SIZE, 0, new AsyncCallback(ReceiveCB), packetState);
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

        protected override int ReceivePacket(NetworkSocket socket)
        {
            int bytesReceived = socket.Socket.Receive(PacketBytes);
            var memoryStream = new MemoryStream();
            NetPacket packet = Utils.BinaryUtils.ByteArrayToObject<NetPacket>(PacketBytes);

            Debug.Log($"Bytes received {bytesReceived}, packet size {packet.size}");

            while(bytesReceived < packet.size)
            {
                int bytes = socket.Socket.Receive(PacketBytes);
                bytesReceived += bytes;
                memoryStream.Write(PacketBytes, 0, bytes);
                Debug.Log($"Writting bytes {bytes}");
            }

            Debug.Log($"Received packet with size {packet.size}");

            //OnPacketReceived?.Invoke();
            return bytesReceived;
        }

        public override int SendPacket(NetPacket packet, NetworkSocket socket)
        {
            //OnPacketSend?.Invoke();

            PacketBytes = Utils.BinaryUtils.ObjectToByteArray(packet);

            Debug.Log($"Server sent packet to IP: {socket.IPAddr}");

            return socket.Socket.Send(PacketBytes);
        }
    }
}