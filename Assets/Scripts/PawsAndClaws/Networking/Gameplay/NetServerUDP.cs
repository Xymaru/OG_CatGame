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
<<<<<<< HEAD
                obj.Socket.SendTo(helloPacket.ToByteArray(), ipenp);
=======
                obj.Socket.Send(helloPacket.ToByteArray(), NetworkPacket.MAX_BUFFER_SIZE, obj.RemoteEP);
>>>>>>> 4f7d1b1b53805ee386b4c59185d91ac0ccdb650f
            }
            else
            {
                ReplicationManager.Instance.ProcessPacket(packet);
<<<<<<< HEAD
                foreach (var client in ConnectedClients)
                {
                    obj.Socket.SendTo(obj.Buffer, client.EndPoint);
                }
            }
            
            ipenp = new IPEndPoint(IPAddress.Any, 0);
            obj.Socket.BeginReceiveFrom(obj.Buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, ref ipenp, new AsyncCallback(ReceiveCB), obj);
        }

        public void SendPacket(NetworkPacket packet)
        {
            byte[] data = packet.ToByteArray();
            foreach (var client in ConnectedClients)
            {
                stateObj.Socket.SendTo(data, client.EndPoint);
            }
=======
            }

            obj.Socket.BeginReceive(new AsyncCallback(ReceiveCB), obj);
>>>>>>> 4f7d1b1b53805ee386b4c59185d91ac0ccdb650f
        }
    }
}