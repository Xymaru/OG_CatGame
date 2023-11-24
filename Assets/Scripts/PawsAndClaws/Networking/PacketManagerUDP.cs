using System.Collections;

using System;

using System.Net;
using System.Net.Sockets;

using UnityEngine;

namespace PawsAndClaws.Networking
{
    class PacketStateUDP
    {
        public Socket socket;
        public EndPoint remoteEP;
        public byte[] buffer;
    }

    public class PacketManagerUDP
    {
        public Action<NetworkPacket> OnPacketReceived;

        public void BeginReceive(Socket socket)
        {
            PacketStateUDP pstate = new PacketStateUDP();
            pstate.socket = socket;
            pstate.buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];
            pstate.remoteEP = new IPEndPoint(IPAddress.Any, NetworkData.Port);

            socket.BeginReceiveFrom(pstate.buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, ref pstate.remoteEP, new AsyncCallback(ReceiveCB), pstate);
        }

        void ReceiveCB(IAsyncResult ar)
        {
            PacketStateUDP pstate = (PacketStateUDP)ar.AsyncState;

            int bytes_read = pstate.socket.EndReceiveFrom(ar, ref pstate.remoteEP);

            if (bytes_read == 0) return;

            NetworkPacket npacket = NetworkPacket.FromByteArray(pstate.buffer);

            // Redirect packet
            OnPacketReceived?.Invoke(npacket);

            pstate.remoteEP = new IPEndPoint(IPAddress.Any, NetworkData.Port);

            // Continue listening
            pstate.socket.BeginReceiveFrom(pstate.buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, ref pstate.remoteEP, new AsyncCallback(ReceiveCB), pstate);
        }
    }
}