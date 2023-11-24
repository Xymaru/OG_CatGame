using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;

namespace PawsAndClaws.Networking
{
    public class NetClientUDP : MonoBehaviour
    {
        Socket _socket;
        PacketManagerUDP _packetManagerUDP = new PacketManagerUDP();
        private Coroutine coroutine;

        NetworkPacket lastPacket;

        public void Start()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _packetManagerUDP.OnPacketReceived += OnPacketReceived;
            _packetManagerUDP.BeginReceive(_socket);
            coroutine = StartCoroutine(SendPacketCoroutine());
        }

        public void Update()
        {
            if (lastPacket != null)
            {
                switch (lastPacket.p_type)
                {
                    case NPacketType.PLAYERPOS:
                        OnPlayerPos((NPPlayerPos)lastPacket);
                        break;
                }

                lastPacket = null;
            }
        }

        void OnPacketReceived(NetworkPacket packet)
        {
            lastPacket = packet;
        }
        void OnPlayerPos(NPPlayerPos packet)
        {
            Debug.Log($"Received position packet from {packet.id}, {packet.team_id}, {packet.slot_id} with coords {packet.x},{packet.y}");

            GameObject player_obj = NetworkData.Teams[packet.team_id].members[packet.slot_id].player_obj;
            Player.NetworkPlayerManager netman = player_obj.GetComponent<Player.NetworkPlayerManager>();
            netman.SetPosition(new Vector2(packet.x, packet.y));
        }

        private void OnDestroy()
        {
            _packetManagerUDP.Close();
            _socket.Close();
        }
        private IEnumerator SendPacketCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                SendPlayerPos();
            }
        }

        public void SendPlayerPos()
        {
            GameObject player_obj = NetworkData.NetSocket.PlayerI.player_obj;
            NPPlayerPos packet = new NPPlayerPos();
            packet.team_id = (ushort)NetworkData.NetSocket.PlayerI.team;
            packet.slot_id = NetworkData.NetSocket.PlayerI.slot;
            packet.x = player_obj.transform.position.x;
            packet.y = player_obj.transform.position.y;

            byte[] data = packet.ToByteArray();

            // Send position to server
            _packetManagerUDP.SendPacket(data);
        }
        
    }
}