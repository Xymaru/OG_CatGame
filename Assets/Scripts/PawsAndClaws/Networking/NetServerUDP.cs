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

        public void Start()
        {
            _netServerTCP = FindObjectOfType<NetServerTCP>();
            Debug.Log($"Starting to receive");
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _packetManagerUDP.BeginReceive(_socket);
        }

        private void Update()
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
            Debug.Log("Sending player pos server!");
            GameObject player_obj = NetworkData.NetSocket.PlayerI.player_obj;

            NPPlayerPos packet = new NPPlayerPos();
            packet.team_id = (ushort)NetworkData.NetSocket.PlayerI.team;
            packet.slot_id = NetworkData.NetSocket.PlayerI.slot;
            packet.x = 0;
            packet.y = 0;

            byte[] data = packet.ToByteArray();

            // Send position to server
            foreach(NetworkSocket socket in _netServerTCP.ConnectedClients)
            {
                if (socket != null)
                {
                    Debug.Log($"Data sent to {socket.EndPoint}");
                    _socket.SendTo(data, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, socket.EndPoint);
                }
            }
        }
        

        
        
    }
}