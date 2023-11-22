using System;
using System.Net;
using System.Net.Sockets;

using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class PacketState
    {
        public NetworkSocket socket;
        public byte[] buffer;
        public int bytesRead;
    }
    public class PacketManagerTCP
    {
        // Socket callbacks
        public Action<NetworkSocket> OnSocketDisconnected;

        // Packet callbacks
        public Action<NetworkPacket> OnPacketReceived;

        public void BeginReceive(NetworkSocket socket)
        {
            PacketState packetState = new PacketState();
            packetState.socket = socket;
            packetState.buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];
            packetState.bytesRead = 0;

            socket.Socket.BeginReceive(packetState.buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, new AsyncCallback(ReceiveCB), packetState);
        }

        void ReceiveCB(IAsyncResult ar)
        {
            PacketState packetState = (PacketState)ar.AsyncState;
            Socket client = packetState.socket.Socket;

            int bytesRead = client.EndReceive(ar);

            // Client disconnected
            if (bytesRead == 0)
            {
                // Socket disconnected callback
                OnSocketDisconnected(packetState.socket);

                // Ignore data and don't continue receiving
                return;
            }

            // Add bytes read
            packetState.bytesRead += bytesRead;

            // Check if packet is completed
            if (packetState.bytesRead == NetworkPacket.MAX_BUFFER_SIZE)
            {
                packetState.bytesRead = 0;

                NetworkPacket npacket = NetworkPacket.FromByteArray(packetState.buffer);

                OnPacketReceived?.Invoke(npacket);
            }

            // Continue reading in offset
            client.BeginReceive(packetState.buffer, packetState.bytesRead, NetworkPacket.MAX_BUFFER_SIZE, 0, new AsyncCallback(ReceiveCB), packetState);
        }
    }
}