using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Sockets;

namespace PawsAndClaws.Networking
{
    public class NetClientTCP : MonoBehaviour
    {
        PacketManagerTCP _packetManagerTCP = new PacketManagerTCP();

        void Start()
        {
            _packetManagerTCP.OnPacketReceived += OnReceivedPacket;
        }

        void Update()
        {

        }

        void OnReceivedPacket(NetworkPacket packet)
        {
            NetworkManager.OnPacketReceived?.Invoke(packet);
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