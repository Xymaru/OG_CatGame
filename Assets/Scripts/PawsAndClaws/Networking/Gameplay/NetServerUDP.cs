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
            state.Socket = new UdpClient(NetworkData.PortUDP);

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
                // TODO: Process and replicate packets
                ReplicationManager.Instance.ProcessPacket(packet);
            }

            obj.Socket.BeginReceive(new AsyncCallback(ReceiveCB), obj);
        }
    }
}