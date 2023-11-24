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

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(NetworkData.ServerEndPoint);

            _packetManagerUDP.OnPacketReceived += OnPacketReceived;

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
            GameObject player_obj = NetworkData.NetSocket.PlayerI.player_obj;

            NPPlayerPos packet = new NPPlayerPos();
            packet.x = player_obj.transform.position.x;
            packet.y = player_obj.transform.position.y;

            byte[] data = packet.ToByteArray();

            // Send position to server
            foreach(NetworkSocket socket in _netServerTCP.ConnectedClients)
            {
                if (socket != null){
                    _socket.SendTo(data, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, socket.Socket.RemoteEndPoint);
                }
            }
        }

        void OnPlayerPos(NPPlayerPos packet)
        {
            Debug.Log($"Received position packet from {packet.id} with coords {packet.x},{packet.y}");

            // Set player position
            GameObject player_obj = _netServerTCP.ConnectedClients[packet.id].PlayerI.player_obj;

            Player.NetworkPlayerManager netman = player_obj.GetComponent<Player.NetworkPlayerManager>();
            netman.SetPosition(new Vector2(packet.x, packet.y));
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
    }
}