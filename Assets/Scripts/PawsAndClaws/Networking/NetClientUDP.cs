using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net.Sockets;

namespace PawsAndClaws.Networking
{
    public class NetClientUDP : MonoBehaviour
    {
        Socket _socket;


        PacketManagerUDP _packetManagerUDP = new PacketManagerUDP();

        public void Start()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Connect(NetworkData.ServerEndPoint);

            _packetManagerUDP.OnPacketReceived += OnPacketReceived;

            _packetManagerUDP.BeginReceive(_socket);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SendPlayerPos();
            }
        }

        private void OnDestroy()
        {
            _socket.Close();
        }

        public void SendPlayerPos()
        {
            GameObject player_obj = NetworkData.NetSocket.PlayerI.player_obj;

            NPPlayerPos packet = new NPPlayerPos();
            packet.x = player_obj.transform.position.x;
            packet.y = player_obj.transform.position.y;

            byte[] data = packet.ToByteArray();

            // Send position to server
            _socket.SendTo(data, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, NetworkData.ServerEndPoint);
        }

        void OnPlayerPos(NPPlayerPos packet)
        {
            // Set player position
            Debug.Log($"Received position {packet.x},{packet.y}");
        }

        void OnPacketReceived(NetworkPacket packet)
        {
            Debug.Log("Received packet");
            switch (packet.p_type)
            {
                case NPacketType.PLAYERPOS:
                    OnPlayerPos((NPPlayerPos)packet);
                    break;
            }
        }
    }
}