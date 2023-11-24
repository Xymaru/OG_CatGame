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

        public void Start()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
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
            Debug.Log("Sending player pos client!");
            GameObject player_obj = NetworkData.NetSocket.PlayerI.player_obj;

            NPPlayerPos packet = new NPPlayerPos();
            packet.team_id = (ushort)NetworkData.NetSocket.PlayerI.team;
            packet.slot_id = NetworkData.NetSocket.PlayerI.slot;
            packet.x = player_obj.transform.position.x;
            packet.y = player_obj.transform.position.y;

            byte[] data = packet.ToByteArray();
            
            Debug.Log($"Data send to {NetworkData.ServerEndPoint}");
            // Send position to server
            _socket.SendTo(data, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, NetworkData.ServerEndPoint);
        }
        
    }
}