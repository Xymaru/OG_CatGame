using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    class PacketStateUDP
    {
        public UdpClient Socket;
        public IPEndPoint RemoteEP;
        public byte[] Buffer;

        ~PacketStateUDP()
        {
            Socket?.Dispose();
        }
    }
    public class NetServerUDP : NetServer
    {
        UdpClient _server = null;

        List<NetworkPacket> m_PacketQueue = new();
        object m_PacketMutex = new();

        List<NetworkPacket> m_SendPacketQueue = new();
        object m_SendPacketMutex = new();
        
        private void Start()
        {
            _server = new UdpClient(NetworkData.PortUDP);

            // Resize list
            for (int i = 0; i < NetServerTCP.MAX_CONNECTIONS; i++)
            {
                ConnectedClients.Add(null);
            }

            BeginReceive();
        }

        private void Update()
        {
            lock (m_PacketMutex)
            {
                foreach(NetworkPacket p in m_PacketQueue)
                {
                    ReplicationManager.Instance.ProcessPacket(p);
                }

                m_PacketQueue.Clear();
            }

            lock (m_SendPacketMutex)
            {
                foreach(NetworkPacket p in m_SendPacketQueue)
                {
                    _broadcast_packet_impl(p);
                }

                m_SendPacketQueue.Clear();
            }
        }

        private void BeginReceive()
        {
            PacketStateUDP state = new PacketStateUDP();
            state.Buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];
            state.Socket = _server;

            state.Socket.BeginReceive(new AsyncCallback(ReceiveCB), state);
        }

        private void ReceiveCB(IAsyncResult ar)
        {
            PacketStateUDP obj = (PacketStateUDP) ar.AsyncState;

            obj.Buffer = obj.Socket.EndReceive(ar, ref obj.RemoteEP);

            NetworkPacket packet = NetworkPacket.FromByteArray(obj.Buffer);
            
            if(packet.p_type == NPacketType.HELLO)
            {
                NPHello client_hello = (NPHello)packet;

                if (ConnectedClients[client_hello.id] == null)
                {
                    // Register the client
                    NetworkSocket client = new NetworkSocket(null, obj.RemoteEP.Address, obj.RemoteEP.Address.ToString(), obj.RemoteEP);

                    Debug.Log($"Connected UDP client from address [{obj.RemoteEP.Address}] and port [{obj.RemoteEP.Port}]");

                    ConnectedClients[client_hello.id] = client;
                }

                // Send the hello packet
                NPHello helloPacket = new NPHello();

                obj.Socket.Send(helloPacket.ToByteArray(), NetworkPacket.MAX_BUFFER_SIZE, obj.RemoteEP);
            }
            else
            {
                lock (m_PacketMutex)
                {
                    m_PacketQueue.Add(packet);
                }
            }

            obj.Socket.BeginReceive(new AsyncCallback(ReceiveCB), obj);
        }

        private void _broadcast_packet_impl(NetworkPacket packet)
        {
            byte[] data = packet.ToByteArray();
            foreach (var client in ConnectedClients)
            {
                if (client == null)
                    continue;

                _server.Send(data, NetworkPacket.MAX_BUFFER_SIZE, (IPEndPoint)client.EndPoint);
            }
        }

        public void BroadcastPacket(NetworkPacket packet)
        {
            lock (m_SendPacketMutex)
            {
                m_SendPacketQueue.Add(packet);
            }
        }
    }
}