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
    public class NetServerUDP : MonoBehaviour
    {
        NetServerTCP _netServerTCP;

        Socket _socket;

        PacketManagerUDP _packetManagerUDP = new PacketManagerUDP();
        private Coroutine coroutine;
        NetworkPacket lastPacket;

        public void Start()
        {
            _netServerTCP = FindObjectOfType<NetServerTCP>();
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
            GameObject player_obj = NetworkData.Teams[packet.team_id].members[packet.slot_id].player_obj;
            Player.NetworkPlayerManager netman = player_obj.GetComponent<Player.NetworkPlayerManager>();
            netman.SetPosition(new Vector2(packet.x, packet.y));
        }
        private IEnumerator SendPacketCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                SendPlayerPos();
            }
        }
        private void OnDestroy()
        {
            _packetManagerUDP.Close();
            _socket.Close();
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