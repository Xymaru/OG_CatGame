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
        public struct StateObj
        {
            public Socket socket;
            public EndPoint endPoint;
            public byte[] buffer;
        }
        private NetworkPacket lastPacketReceived = null;
        private StateObj stateObj;
       

        private void Awake()
        {
            stateObj.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, NetworkData.PortUDP);
            stateObj.socket.Bind(endPoint);

            BeginReceive();
        }

        private void BeginReceive()
        {
            EndPoint ipenp = new IPEndPoint(IPAddress.Any, 0);
            stateObj.socket.BeginReceiveFrom(stateObj.buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, ref ipenp, new AsyncCallback(ReceiveCB), null);
        }

        private void ReceiveCB(IAsyncResult ar)
        {
            StateObj obj = (StateObj) ar.AsyncState;
            EndPoint ipenp = new IPEndPoint(IPAddress.Any, 0);
            int bytesRecieved = obj.socket.EndReceiveFrom(ar, ref ipenp);
            if (bytesRecieved == 0)
                return;

            NetworkPacket packet = NetworkPacket.FromByteArray(obj.buffer);
            if(packet.p_type == NPacketType.HELLO)
            {

            }
        }
        private void OnPacketReceived(NetworkPacket packet)
        {
            lastPacketReceived = packet;
        }
    }
}