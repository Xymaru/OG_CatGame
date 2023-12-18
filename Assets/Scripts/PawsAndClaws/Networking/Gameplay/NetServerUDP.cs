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
    public class NetServerUDP : NetServer
    {
        private PacketStateUDP stateObj = new PacketStateUDP();

        private void Start()
        {
            stateObj.Buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];
            stateObj.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, NetworkData.PortUDP);
            stateObj.Socket.Bind(endPoint);

            BeginReceive();
        }

        private void BeginReceive()
        {
            EndPoint ipenp = new IPEndPoint(IPAddress.Any, 0);
            stateObj.Socket.BeginReceiveFrom(stateObj.Buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, ref ipenp, new AsyncCallback(ReceiveCB), stateObj);
        }

        private void ReceiveCB(IAsyncResult ar)
        {
            PacketStateUDP obj = (PacketStateUDP) ar.AsyncState;
            EndPoint ipenp = new IPEndPoint(IPAddress.Any, 0);
            int bytesReceived = obj.Socket.EndReceiveFrom(ar, ref ipenp);
            if (bytesReceived == 0)
                return;

            NetworkPacket packet = NetworkPacket.FromByteArray(obj.Buffer);
            
            if(packet.p_type == NPacketType.HELLO)
            {
                // Register the client
                IPEndPoint endPoint = (IPEndPoint)ipenp;
                NetworkSocket client = new NetworkSocket(null, endPoint.Address, endPoint.Address.ToString(), ipenp);
                Debug.Log($"Connected client from address [{endPoint.Address}]");
                ConnectedClients.Add(client);
                
                // Send the hello packet
                NPHello helloPacket = new NPHello();
                stateObj.Socket.SendTo(helloPacket.ToByteArray(), ipenp);
            }
            else
            {
                // TODO: Process and replicate packets
                ReplicationManager.Instance.ProcessPacket(packet);
                
            }
            
            ipenp = new IPEndPoint(IPAddress.Any, 0);
            stateObj.Socket.BeginReceiveFrom(stateObj.Buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, ref ipenp, new AsyncCallback(ReceiveCB), stateObj);
        }
    }
}