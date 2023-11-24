using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PawsAndClaws.Game;
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
        private Thread _thread;
        private object _mutex = new();

        public void BeginReceive(Socket socket)
        {
            PacketStateUDP pstate = new PacketStateUDP();
            pstate.socket = socket;
            pstate.buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];
            if (NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                pstate.remoteEP = NetworkData.ServerEndPoint;
            }
            else
            {
                pstate.remoteEP = new IPEndPoint(IPAddress.Any, NetworkData.Port);
            }

            _thread = new Thread(() => ReceiveCB(pstate));
            _thread.Start();
        }

        void ReceiveCB(PacketStateUDP pstate)
        {
            while (true)
            {
                int bytes_read = pstate.socket.ReceiveFrom(pstate.buffer, SocketFlags.None, ref pstate.remoteEP);
                Debug.Log($"Started receiving from {pstate.remoteEP}, {bytes_read}");
                if (bytes_read == 0) return;

                NetworkPacket npacket = NetworkPacket.FromByteArray(pstate.buffer);
                // Redirect packet
                OnPacketReceived(npacket);


                if (NetworkData.NetSocket.NetCon == NetCon.Host)
                {
                    pstate.remoteEP = new IPEndPoint(IPAddress.Any, NetworkData.Port);
                }

                // Continue listening
                //pstate.socket.BeginReceiveFrom(pstate.buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, ref pstate.remoteEP, new AsyncCallback(ReceiveCB), pstate);
            }
        }

        void OnPacketReceived(NetworkPacket packet)
        {
            switch (packet.p_type)
            {
                case NPacketType.PLAYERPOS:
                    OnPlayerPos((NPPlayerPos)packet);
                    break;
            }
        }

        void OnPlayerPos(NPPlayerPos packet)
        {
            Debug.Log(
                $"Received position packet from {packet.id}, {packet.team_id}, {packet.slot_id} with coords {packet.x},{packet.y}");

            // Set player position
            GameObject player_obj = NetworkData.Teams[packet.team_id].members[packet.slot_id].player_obj;
            Player.NetworkPlayerManager netman = player_obj.GetComponent<Player.NetworkPlayerManager>();
            netman.SetPosition(new Vector2(packet.x, packet.y));
        }

        ~PacketManagerUDP()
        {
            if (_thread.IsAlive)
            {
                _thread.Abort();
            }
        }
    }
}