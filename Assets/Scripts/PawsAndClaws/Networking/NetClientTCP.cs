using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Sockets;

namespace PawsAndClaws.Networking
{
    public class NetClientTCP : MonoBehaviour
    {
        public PlayerInfo[] ConnectedClients = new PlayerInfo[6];

        PacketManagerTCP _packetManagerTCP = new PacketManagerTCP();

        void Start()
        {
            _packetManagerTCP.OnPacketReceived += OnReceivedPacket;
            _packetManagerTCP.OnSocketDisconnected += OnSocketDisconnect;

            // Begin receiving from server
            _packetManagerTCP.BeginReceive(NetworkData.NetSocket);
        }

        void Update()
        {

        }

        void OnReceivedPacket(NetworkPacket packet)
        {
            NetworkManager.OnPacketReceived?.Invoke(packet);
        }

        void OnSocketDisconnect(NetworkSocket socket)
        {
            // Handle server disconnection

        }

        private void CloseConnections()
        {
            if (NetworkData.NetSocket.Socket.Connected)
            {
                NetworkData.NetSocket.Socket.Shutdown(SocketShutdown.Both);
            }

            NetworkData.NetSocket.Socket.Close();

            Debug.Log("Closing connection socket.");
        }

        private void OnDestroy()
        {
            CloseConnections();
        }
    }
}