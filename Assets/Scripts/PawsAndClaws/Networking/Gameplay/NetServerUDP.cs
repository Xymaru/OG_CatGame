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

        private void Awake()
        {
            _server = new UdpClient(NetworkData.PortUDP);
        }

        private void Start()
        {
            

            // Resize list
            for (int i = 0; i < NetServerTCP.MAX_CONNECTIONS; i++)
            {
                ConnectedClients.Add(null);
            }

            BeginReceive();
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

                // Register the client
                NetworkSocket client = new NetworkSocket(null, obj.RemoteEP.Address, obj.RemoteEP.Address.ToString(), obj.RemoteEP);

                Debug.Log($"Connected client from address [{obj.RemoteEP.Address}] and port [{obj.RemoteEP.Port}]");

                ConnectedClients[client_hello.id] = client;
                
                // Send the hello packet
                NPHello helloPacket = new NPHello();

                obj.Socket.Send(helloPacket.ToByteArray(), NetworkPacket.MAX_BUFFER_SIZE, obj.RemoteEP);
            }
            else
            {
                ReplicationManager.Instance.ProcessPacket(packet);

                foreach (var client in ConnectedClients)
                {
                    obj.Socket.Send(obj.Buffer, NetworkPacket.MAX_BUFFER_SIZE, (IPEndPoint)client.EndPoint);
                }
            }

            obj.Socket.BeginReceive(new AsyncCallback(ReceiveCB), obj);
        }

        public void SendPacket(NetworkPacket packet)
        {
            byte[] data = packet.ToByteArray();
            foreach (var client in ConnectedClients)
            {
                _server.Send(data, NetworkPacket.MAX_BUFFER_SIZE, (IPEndPoint)client.EndPoint);
            }
        }
    }
}